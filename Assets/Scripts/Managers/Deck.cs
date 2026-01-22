using UnityEngine;
using System.Collections.Generic;

public class Deck : MonoBehaviour
{
    public static Deck Instance;

    [SerializeField] private List<CardData> startingDeck;

    [SerializeField] private List<CardData> drawPile = new List<CardData>();
    public int DrawPileCount => drawPile.Count;

    [SerializeField] private List<CardData> discardPile = new List<CardData>();
    public int DiscardPileCount => discardPile.Count;

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
        if (drawPile.Count == 0)
        {
            if (discardPile.Count == 0)
            {
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
    }

    private void Shuffle()
    {
        for (int i = drawPile.Count - 1; i > 0; i--)
        {
            int r = Random.Range(0, i + 1);
            CardData temp = drawPile[i];
            drawPile[i] = drawPile[r];
            drawPile[r] = temp;
        }
    }

    private void ReshuffleDiscard()
    {
        drawPile.AddRange(discardPile);
        discardPile.Clear();
        Shuffle();
    }
}