using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem.Controls;
using System;

public class EnemyDeck : MonoBehaviour
{
    public static EnemyDeck Instance;

    [SerializeField] private List<CardData> startingDeck;

    [SerializeField] private List<CardData> drawPile = new List<CardData>();
    public int DrawPileCount => drawPile.Count;

    [SerializeField] private List<CardData> discardPile = new List<CardData>();
    public int DiscardPileCount => discardPile.Count;

    // Support multiple decks
    [System.Serializable]
    public class EnemyDeckPreset
    {
        public string deckName;
        public List<CardData> cards;
    }

    [SerializeField] private List<EnemyDeckPreset> deckPresets = new List<EnemyDeckPreset>();

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Set drawpile
        drawPile.AddRange(startingDeck);
        Shuffle();
    }

    // DrawEffect method
    public void DrawCard()
    {
        CardData drawnCard = Draw();

        if (drawnCard != null)
        {
            Hand.Instance.AddCard(drawnCard);
        }
        else
        {
            Debug.Log("No cards left to draw!");
        }
    }

    public CardData Draw()
    {
        Debug.Log($"DRAW called: DrawPile={drawPile.Count}, DiscardPile={discardPile.Count}");

        if (drawPile.Count == 0)
        {
            Debug.Log("Draw pile empty - reshuffling discard");
            if (discardPile.Count == 0)
            {
                Debug.LogWarning("Both piles empty!");
                return null;
            }
            ReshuffleDiscard();
        }

        CardData card = drawPile[0];
        drawPile.RemoveAt(0);
        return card;
    }

    public void Discard(CardData card)
    {
        discardPile.Add(card);
        Debug.Log($"Enemy discarded: {card.Name} - Discard pile: {discardPile.Count}");
    }

    private void Shuffle()
    {
        for (int i = drawPile.Count - 1; i > 0; i--)
        {
            int r = UnityEngine.Random.Range(0, i + 1);
            CardData temp = drawPile[i];
            drawPile[i] = drawPile[r];
            drawPile[r] = temp;
        }
    }

    private void ReshuffleDiscard()
    {
        Debug.Log($"RESHUFFLE: Moving {discardPile.Count} cards from discard to draw");

        drawPile.AddRange(discardPile);
        discardPile.Clear();
        Shuffle();

        Debug.Log($"After reshuffle: Draw={drawPile.Count}, Discard={discardPile.Count}");
    }

    public void ResetDeck()
    {
        drawPile.Clear();
        discardPile.Clear();
        drawPile.AddRange(startingDeck);
        Shuffle();

        Debug.Log($"Enemy deck reset - {drawPile.Count} cards");
    }

    public void LoadDeck(string deckName)
    {
        EnemyDeckPreset preset = deckPresets.Find(d => d.deckName == deckName);

        if (preset != null)
        {
            startingDeck = new List<CardData>(preset.cards);

            // Reset piles with new deck
            drawPile.Clear();
            discardPile.Clear();
            drawPile.AddRange(startingDeck);
            Shuffle();

            Debug.Log($"Loaded enemy deck: {deckName} ({startingDeck.Count} cards)");
        }
        else
        {
            Debug.LogError($"Deck preset '{deckName}' not found!");
        }
    }
}