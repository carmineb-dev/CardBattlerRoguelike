using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    // === REFERENCES ===
    [SerializeField] private Player player;

    public Player Player => player;

    [SerializeField] private PlayerBlockUI playerBlockUI;

    [SerializeField] private Enemy enemy;
    public Enemy Enemy => enemy;

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
        player.Initialize();
        enemy.Initialize();
    }

    public void EndTurn()
    {
        // ENEMY TURN
        // Enemy turn (placeholder)
        if (Enemy.skipTurns > 0)
        {
            Debug.Log("Enemy turn skipped!");
            // Don't call enemy AI
        }
        else
        {
            List<CardData> enemyCards = Enemy.ChooseCardsToPlay();

            // Queue enemy cards (same as player)
            foreach (CardData card in enemyCards)
            {
                // For now create fake card instance using null
                QueueEnemyCard(card, card.Priority);
            }
        }
        // RESOLUTION
        StartCoroutine(ResolveTurn());
    }

    private IEnumerator ResolveTurn()
    {
        Debug.Log("=== RESOLUTION PHASE START ===");

        // Sort cards by priority
        cardsToResolve.Sort((a, b) => a.resolvedPriority.CompareTo(b.resolvedPriority));

        // Execute cards in order
        foreach (CardPlayData playData in cardsToResolve)
        {
            Debug.Log($"Resolving: {playData.cardData.Name} (Priority {playData.cardData.Priority})");

            // Execute effect
            playData.cardData.Effect.Execute(playData.caster, playData.target, playData.cardData.Value);

            // Remove from hand if player
            if (playData.cardInstance != null && playData.caster == Player)
            {
                Hand.Instance.RemoveCard(playData.cardInstance);
            }

            //Pause for visual feedback
            yield return new WaitForSeconds(0.5f);
        }
        // Clear resolved cards
        cardsToResolve.Clear();
        DestroyPlayedCards();

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
        CardPlayData playData = new CardPlayData
        {
            cardData = cardData,
            cardInstance = null, // Enemy has no Card UI
            caster = Enemy,
            target = Player,
            resolvedPriority = priority
        };

        cardsToResolve.Add(playData);
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

    private void DestroyPlayedCards()
    {
        //Find every played card (with alpha = 0.5)
        Card[] allCards = FindObjectsByType<Card>(FindObjectsSortMode.None);
        foreach (Card card in allCards)
        {
            CanvasGroup cg = card.GetComponent<CanvasGroup>();
            if (cg != null && cg.alpha < 1f)
            {
                Destroy(card.gameObject);
            }
        }
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
}