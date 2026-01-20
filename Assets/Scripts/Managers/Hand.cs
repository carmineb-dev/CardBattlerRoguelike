using UnityEngine;
using System.Collections.Generic;

public class Hand : MonoBehaviour
{
    public static Hand Instance;

    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform handTransform;

    private List<Card> cardsInHand = new List<Card>();

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
    }

    public void RemoveCard(Card card)
    {
        cardsInHand.Remove(card);

        Destroy(card.gameObject);
    }

    public void ClearHand()
    {
        var copy = new List<Card>(cardsInHand);

        foreach (Card card in copy)
        {
            RemoveCard(card);
        }
    }

    public int CardCount => cardsInHand.Count;
}