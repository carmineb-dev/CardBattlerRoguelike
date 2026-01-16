using UnityEngine;
using System.Collections.Generic;

public class Hand : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform handTransform;

    private List<Card> cardsInHand = new List<Card>();

    public void AddCard(CardData cardData)
    {
        // Spawn card prefab
        GameObject cardObj = Instantiate(cardPrefab, handTransform);
        Card cardScript = cardObj.GetComponent<Card>();

        // Assign data automatically
        cardScript.Initialize(cardData);

        cardsInHand.Add(cardScript);
    }

    public void RemoveCard(Card card)
    {
        cardsInHand.Remove(card);
        Destroy(card.gameObject);
    }

    public void ClearHand()
    {
        foreach (Card card in cardsInHand)
        {
            Destroy(card.gameObject);
        }

        cardsInHand.Clear();
    }

    public int CardCount => cardsInHand.Count;
}