using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    // === UI ===
    [SerializeField] private EnemyHealthUI healthUI;

    // === HAND ===
    private List<CardData> enemyHand = new List<CardData>();

    private void Awake()
    {
        characterName = "Enemy";
    }

    public override void Initialize()
    {
        // Stats
        currentHp = maxHp;
        currentMana = maxMana;
        currentBlock = 0;

        // UI
        healthUI.InitializeUI(maxHp);

        enemyHand.Clear();

        // Draw 5 cards
        for (int i = 0; i < 5; i++)
        {
            CardData card = EnemyDeck.Instance.Draw();
            if (card != null)
            {
                enemyHand.Add(card);
            }
        }
        Debug.Log($"Enemy drew {enemyHand.Count} cards");
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        healthUI.UpdateUI(currentHp, maxHp);
    }

    public override void TakePiercingDamage(int damage)
    {
        base.TakePiercingDamage(damage);
        healthUI.UpdateUI(currentHp, maxHp);
    }

    protected override void Die()
    {
        base.Die();
        Debug.Log("Enemy defeated!");
    }

    public List<CardData> ChooseCardsToPlay()
    {
        List<CardData> cardsToPlay = new List<CardData>();

        // Play cards until mana is finished
        while (currentMana > 0 && enemyHand.Count > 0)
        {
            // Filter playable cards (cost <= mana)
            List<CardData> playableCards = new List<CardData>();

            foreach (CardData card in enemyHand)
            {
                if (card.Cost <= currentMana)
                {
                    playableCards.Add(card);
                }
            }

            // No playable cards -> stop
            if (playableCards.Count == 0)
            {
                break;
            }

            // Pick random playable card
            int randomIndex = Random.Range(0, playableCards.Count);
            CardData chosenCard = playableCards[randomIndex];

            // Add card to play list
            cardsToPlay.Add(chosenCard);

            // Remove from hand
            enemyHand.Remove(chosenCard);

            // Spend mana
            SpendMana(chosenCard.Cost);

            Debug.Log($"Enemy queued: {chosenCard.Name} (Cost: {chosenCard.Cost})");
        }

        Debug.Log($"Enemy chose {cardsToPlay.Count} cards to play");
        return cardsToPlay;
    }

    public void DrawToHandSize(int targetSize)
    {
        int cardsToDraw = targetSize - enemyHand.Count;

        for (int i = 0; i < cardsToDraw; i++)
        {
            CardData card = EnemyDeck.Instance.Draw();
            if (card != null)
            {
                enemyHand.Add(card);
            }
            else
            {
                Debug.LogWarning("Enemy deck empty!");
                break;
            }
        }
        Debug.Log($"Enemy drew {cardsToDraw} cards, hand size: {enemyHand.Count}");
    }
}