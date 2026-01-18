using UnityEngine;
using System.Collections.Generic;

public class Hand : MonoBehaviour
{
    public static Hand instance;

    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform handTransform;

    private List<Card> cardsInHand = new List<Card>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
    }

    public void RemoveCard(Card card)
    {
        cardsInHand.Remove(card);
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