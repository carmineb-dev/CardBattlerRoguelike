/*/
using UnityEngine;
using System.Collections.Generic;

public class HandUI : MonoBehaviour
{
    [Header("Layout Settings")]
    [SerializeField] private float cardSpacing = 140f; // Spacing between cards

    [SerializeField] private float arcHeight = 40f;
    [SerializeField] private float maxSpread = 900f;
    [SerializeField] private float cardRotationFactor = 4f;

    [Header("Hover Settings")]
    [SerializeField] private float hoverYOffset = 80f;

    [SerializeField] private float hoverScale = 1.3f; // Zoom hover

    private List<Card> cards = new List<Card>();
    private Card hoveredCard = null;

    public void UpdateCardPositions(List<Card> cardsInHand)
    {
        cards = cardsInHand;
        RepositionCards();
    }

    private void RepositionCards()
    {
        int cardCount = cards.Count;
        if (cardCount == 0) return;

        // Calculate dynamic spacing
        float dynamicSpacing = cardSpacing;
        float totalWidth = dynamicSpacing * (cardCount - 1);

        if (totalWidth > maxSpread)
        {
            // Reduce spacing when too much cards
            dynamicSpacing = maxSpread / (cardCount - 1);
        }

        // Hand center
        float startX = -(dynamicSpacing * (cardCount - 1)) / 2f;

        for (int i = 0; i < cardCount; i++)
        {
            Card card = cards[i];

            // Skip hover card
            if (card == hoveredCard) continue;

            // X Position (horizontal spread)
            float xPos = startX + (i * dynamicSpacing);

            // Y Position (arc)
            float normalizedPos = (cardCount == 1) ? 0.5f : (float)i / (cardCount - 1); // 0 to 1
            float yOffset = Mathf.Sin(normalizedPos * Mathf.PI) * arcHeight;
            float yPos = -yOffset; // Negative = descending arc

            // Rotation
            float centerOffset = normalizedPos - 0.5f; // -0.5 a +0.5
            float rotation = centerOffset * cardRotationFactor * Mathf.Min(cardCount, 8); // Max rotation with 8 cards

            // Z order (cards on right above those on the left)
            int siblingIndex = i;

            // Apply transform
            card.transform.localPosition = new Vector3(xPos, yPos, 0);
            card.transform.localRotation = Quaternion.Euler(0, 0, rotation);
            card.transform.SetSiblingIndex(siblingIndex);
        }
    }

private void RepositionCards()
    {
        int cardCount = cards.Count;
        if (cardCount == 0) return;

        Debug.Log($"Repositioning {cardCount} cards");

        for (int i = 0; i < cardCount; i++)
        {
            Card card = cards[i];

            // TEST: spacing fisso 150, no arc, no rotation
            float xPos = (i - cardCount / 2f) * 150f;

            card.transform.localPosition = new Vector3(xPos, 0, 0);
            card.transform.localRotation = Quaternion.identity;
            card.transform.localScale = Vector3.one;

            Debug.Log($"Card {i} positioned at X={xPos}");
        }
    }

    public void OnCardHoverEnter(Card card)
    {
        hoveredCard = card;

        // Took card on close-up
        card.transform.SetAsLastSibling();

        // Zoom + lift
        card.transform.localScale = Vector3.one * hoverScale;

        // Lift above other cards
        Vector3 currentPos = card.transform.localPosition;
        card.transform.localPosition = new Vector3(currentPos.x, hoverYOffset, currentPos.z);

        // Reset rotation (straight card when hover)
        card.transform.localRotation = Quaternion.identity;
    }

    public void OnCardHoverExit(Card card)
    {
        if (hoveredCard == card)
        {
            hoveredCard = null;
        }

        // Reset scale
        card.transform.localScale = Vector3.one;

        // Reposition with normal layout
        RepositionCards();
    }
}
/*/