using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    // === CHARACTERS ===
    [SerializeField] private Player player;

    public Player Player => player;

    [SerializeField] private Enemy enemy;
    public Enemy Enemy => enemy;

    // === UI ===
    [SerializeField] private PlayerBlockUI playerBlockUI;

    [SerializeField] private EnemyIntentUI enemyIntentUI;

    private List<CardPlayData> cardsToResolve = new List<CardPlayData>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        InitializeCombat();
    }

    private void InitializeCombat()
    {
        // Load appropriate deck for fight
        string deckName = GameManager.Instance.GetEnemyDeckForFight();
        EnemyDeck.Instance.LoadDeck(deckName);

        // Set enemy strategy
        Enemy.SetStrategy(GameManager.Instance.GetEnemyStrategyForFight());

        player.Initialize();
        enemy.Initialize();

        // === Enemy prechoose for turn 1===
        Enemy.PreChooseCardsForTurn();
        enemyIntentUI.ShowIntent(Enemy.GetPreChosenCards());

        Debug.Log("Combat initialized - Enemy intent shown for Turn 1");
    }

    public void EndTurn()
    {
        List<CardData> enemyCards = Enemy.GetPreChosenCards();

        // Queue enemy cards (same as player)
        foreach (CardData card in enemyCards)
        {
            // For now create fake card instance using null
            QueueEnemyCard(card, card.Priority);
        }

        enemyIntentUI.HideIntent();

        Enemy.ClearPreChosenCards();

        // RESOLUTION
        StartCoroutine(ResolveTurn());
    }

    private IEnumerator ResolveTurn()
    {
        Debug.Log("=== RESOLUTION PHASE START ===");

        // Sort cards by priority
        cardsToResolve.Sort((a, b) =>
        {
            int priorityCompare = a.resolvedPriority.CompareTo(b.resolvedPriority);

            // If priority is the same -> random coinflip
            if (priorityCompare == 0)
            {
                return Random.Range(0, 2) == 0 ? -1 : 1;
            }
            return priorityCompare;
        });

        // Visual cards reorder
        for (int i = 0; i < cardsToResolve.Count; i++)
        {
            CardPlayData playData = cardsToResolve[i];

            // Only player cards
            if (playData.cardInstance != null)
            {
                playData.cardInstance.transform.SetSiblingIndex(cardsToResolve.Count - i);
            }
        }

        yield return new WaitForSeconds(0.3f);

        // Execute cards in order
        foreach (CardPlayData playData in cardsToResolve)
        {
            Debug.Log($"Resolving: {playData.cardData.Name} (Priority {playData.cardData.Priority})");

            // Execute effect
            playData.cardData.Effect.Execute(playData.caster, playData.target, playData.cardData.Value);

            // Check if enemy died
            if (Enemy.currentHp <= 0)
            {
                Debug.Log("Enemy defeated!");
                ShowVictoryScreen();
                yield break;
            }

            // Fade out cards
            if (playData.cardInstance != null)
            {
                // Remove from hand if is player
                if (playData.caster == Player)
                {
                    Hand.Instance.RemoveCard(playData.cardInstance);
                    Deck.Instance.Discard(playData.cardData); // Player discard
                }
                else
                {
                    EnemyDeck.Instance.Discard(playData.cardData); // Enemy discard
                }
                playData.cardInstance.TriggerFadeOut();
            }

            // Pause for visual feedback
            yield return new WaitForSeconds(1f);
        }

        // Clear resolved cards
        cardsToResolve.Clear();

        // Hide intent after resolution
        enemyIntentUI.HideIntent();

        Debug.Log("=== RESOLUTION PHASE END ===");

        yield return new WaitForSeconds(0.3f);

        // Cleanup turn
        Player.ResetBlock();
        Enemy.ResetBlock();
        Player.RefillMana();
        Enemy.RefillMana();

        // Process turn-based buffs
        ProcessTurnBuffs();

        // Reset counter stance
        Player.hasCounterStance = false;
        Enemy.hasCounterStance = false;

        // Draw back to 5 cards if possible
        int cardsToDraw = 5 - Hand.Instance.CardCount;

        for (int i = 0; i < cardsToDraw; i++)
        {
            // Safety check: if deck is empty, stop
            if (Deck.Instance.DrawPileCount == 0 && Deck.Instance.DiscardPileCount == 0)
            {
                Debug.LogWarning("No more cards to draw!");
                break;
            }

            Deck.Instance.DrawCard();
        }
        // Enemy draw back to 5
        Enemy.DrawToHandSize(5);

        // ENEMY TURN
        // Enemy turn (placeholder)
        if (Enemy.skipTurns > 0)
        {
            Debug.Log("Enemy turn skipped!");
            enemyIntentUI.HideIntent();
            // Don't call enemy AI
        }
        else
        {
            Enemy.PreChooseCardsForTurn();
            enemyIntentUI.ShowIntent(Enemy.GetPreChosenCards());
        }

        Debug.Log("=== NEW TURN START ===");
    }

    public void QueueCard(Card cardInstance, Character caster, Character target, int finalPriority)
    {
        CardPlayData playData = new CardPlayData
        {
            cardData = cardInstance != null ? cardInstance.cardData : null, // Handle null
            cardInstance = cardInstance, // Can be null
            caster = caster,
            target = target,
            resolvedPriority = finalPriority
        };

        cardsToResolve.Add(playData);
    }

    public void QueueEnemyCard(CardData cardData, int priority)
    {
        // Spawn Card UI for enemy
        GameObject cardObj = Instantiate(Hand.Instance.cardPrefab);
        Card cardScript = cardObj.GetComponent<Card>();

        // Initialize enemy card data
        cardScript.Initialize(cardData, Enemy);

        // Starting position (outside the screen)
        Canvas mainCanvas = FindFirstObjectByType<Canvas>();
        cardScript.transform.SetParent(mainCanvas.transform);
        cardScript.transform.position = new Vector3(Screen.width + 200, Screen.height / 2f, 0);

        CardPlayData playData = new CardPlayData
        {
            cardData = cardData,
            cardInstance = cardScript,
            caster = Enemy,
            target = Player,
            resolvedPriority = priority
        };

        cardsToResolve.Add(playData);

        //Trigger slide animation
        cardScript.StartCoroutine(cardScript.SlideToPosition(isEnemy: true));

        Debug.Log($"Enemy queued: {cardData.Name} (Priority {priority})");
    }

    [System.Serializable]
    public struct CardPlayData
    {
        public CardData cardData; // Card Data
        public Card cardInstance; // Reference to the UI object
        public Character caster;
        public Character target;
        public int resolvedPriority;
    }

    private void ProcessTurnBuffs()
    {
        // Iron Wall regen
        if (Player.ironWallTurns > 0)
        {
            Player.GainBlock(Player.ironWallBlockPerTurn);
            playerBlockUI.UpdateUI(Player.ironWallBlockPerTurn);
            Player.ironWallTurns--;
            Debug.Log($"Iron Wall: +{Player.ironWallBlockPerTurn} block ({Player.ironWallTurns} turns left)");
        }

        // Enemy skip check (Time Warp)
        if (Enemy.skipTurns > 0)
        {
            Enemy.skipTurns--;
            Debug.Log($"Enemy skipped turn ({Enemy.skipTurns} skips left");
            // TODO: Skip enemy AI logic
        }

        // Reset priority boost
        if (Player.hasParryActive && !Player.parryActivatedThisTurn)
        {
            Player.priorityBoostRemaining = 0;
            Player.hasParryActive = false;
        }
        if (Player.parryActivatedThisTurn)
        {
            Player.parryActivatedThisTurn = false;
        }

        if (Enemy.hasParryActive)
        {
            Enemy.priorityBoostRemaining = 0;
            Enemy.priorityBoostValue = 0;
            Enemy.hasParryActive = false;
        }
    }

    private void ShowVictoryScreen()
    {
        // Enable victory UI (placeholder)
        Debug.Log("VICTORY! Show next fight button");

        // Cleanup carte non risolte
        CleanupUnresolvedCards();

        // Placeholder: auto next fight
        StartCoroutine(AutoNextFight());
    }

    private void CleanupUnresolvedCards()
    {
        // Destroy alla cards in scene
        Card[] allCards = FindObjectsByType<Card>(FindObjectsSortMode.None);

        foreach (Card card in allCards)
        {
            Destroy(card.gameObject);
        }

        // Clear queue
        cardsToResolve.Clear();

        Debug.Log("Cleaned up unresolved cards");
    }

    private IEnumerator AutoNextFight()
    {
        yield return new WaitForSeconds(2f);

        if (GameManager.Instance.IsBossFight())
        {
            Debug.Log("YOU WIN THE GAME!");
            // TODO: Win screen
        }
        else
        {
            GameManager.Instance.NextFight();

            // Load deck for new fight
            string deckName = GameManager.Instance.GetEnemyDeckForFight();
            EnemyDeck.Instance.LoadDeck(deckName);

            // Set strategy
            Enemy.SetStrategy(GameManager.Instance.GetEnemyStrategyForFight());

            // Reset decks
            Deck.Instance.ResetDeck();

            // Clear Hand
            Hand.Instance.ClearHand();

            // Reinitialize combat
            Player.Initialize();
            Enemy.Initialize();

            // Enemy pre-choose cards
            Enemy.PreChooseCardsForTurn();
            enemyIntentUI.ShowIntent(Enemy.GetPreChosenCards());

            // Update UI
            FindFirstObjectByType<FightCounterUI>().UpdateUI();

            Debug.Log("Next fight started!");
        }
    }
}