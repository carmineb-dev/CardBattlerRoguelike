using UnityEngine;
using System.Collections.Generic;

public class Hand : MonoBehaviour
{
    public static Hand Instance;

    public GameObject cardPrefab;
    [SerializeField] private Transform handTransform;

    private List<Card> cardsInHand = new List<Card>();

    public int CardCount => cardsInHand.Count;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCard(CardData cardData)
    {
        // Spawn card prefab
        GameObject cardObj = Instantiate(cardPrefab, handTransform);
        Card cardScript = cardObj.GetComponent<Card>();

        // Assign data automatically
        cardScript.Initialize(cardData, CombatManager.Instance.Player);

        cardsInHand.Add(cardScript);

        UpdateLayout();
    }

    public void RemoveCard(Card card)
    {
        cardsInHand.Remove(card);

        if (card != null && !card.IsPlayed())
        {
            UpdateLayout();
        }
    }

    public void ClearHand()
    {
        var copy = new List<Card>(cardsInHand);

        foreach (Card card in copy)
        {
            RemoveCard(card);
        }
    }

    public void UpdateLayout()
    {
        int cardCount = cardsInHand.Count;
        if (cardCount == 0) return;

        // Dynamic spacing based on card count
        float baseSpacing = 150f; // Standard spacing with 5 cards
        float minSpacing = 80f;   // Minimum spacing with light overlap

        float spacing = baseSpacing;

        if (cardCount > 5)
        {
            // Proportionally reduce spacing
            spacing = Mathf.Max(minSpacing, baseSpacing * (5f / cardCount));
        }

        // Calculate total width
        float totalWidth = spacing * (cardCount - 1);

        // Starting position (in the middle)
        float startX = -totalWidth / 2f;

        // Position each card
        for (int i = 0; i < cardCount; i++)
        {
            Card card = cardsInHand[i];

            // Skip if played
            if (card == null || card.IsPlayed())
            {
                continue;
            }

            float xPos = startX + (i * spacing);
            float yPos = 245f; // Flat

            card.transform.localPosition = new Vector3(xPos, yPos, 0f);
            card.transform.localRotation = Quaternion.identity; // No rotation
            card.transform.localScale = Vector3.one;

            // Z-order (cards on the right above left ones)
            card.transform.SetSiblingIndex(i);
        }

        Debug.Log($"Hand layout updated: {cardCount} cards, spacing: {spacing:F1}px");
    }
}