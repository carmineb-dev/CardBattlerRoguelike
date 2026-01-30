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
        StartCoroutine(ResolveTurn());
    }

    private IEnumerator ResolveTurn()
    {
        Debug.Log("=== RESOLUTION PHASE START ===");

        // Sort cards by priority
        cardsToResolve.Sort((a, b) => a.cardData.Priority.CompareTo(b.cardData.Priority));

        // Execute cards in order
        foreach (CardPlayData playData in cardsToResolve)
        {
            Debug.Log($"Resolving: {playData.cardData.Name} (Priority {playData.cardData.Priority})");

            // Execute effect
            playData.cardData.Effect.Execute(playData.caster, playData.target, playData.cardData.Value);

            // Remove from hand if player
            if (playData.caster == Player)
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

        // Enemy turn (placeholder)
        Player.TakeDamage(5);
        Debug.Log("Enemy attacked for 5 damage");

        yield return new WaitForSeconds(0.3f);

        // Cleanup turn
        Player.ResetBlock();
        Enemy.ResetBlock();
        Player.RefillMana();

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

        Debug.Log("=== NEW TURN START ===");
    }

    public void QueueCard(Card cardInstance, Character caster, Character target)
    {
        CardPlayData playData = new CardPlayData
        {
            cardData = cardInstance.cardData,
            cardInstance = cardInstance,
            caster = caster,
            target = target,
        };

        cardsToResolve.Add(playData);
    }

    [System.Serializable]
    public struct CardPlayData
    {
        public CardData cardData; // Card Data
        public Card cardInstance; // Reference to the UI object
        public Character caster;
        public Character target;
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
}