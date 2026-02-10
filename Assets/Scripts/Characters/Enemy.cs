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

    public CardData ChooseRandomCard()
    {
        if (enemyHand.Count == 0)
        {
            Debug.Log("Enemy has no cards!");
            return null;
        }

        // Pick random card
        int randomIndex = Random.Range(0, enemyHand.Count);
        CardData chosenCard = enemyHand[randomIndex];

        // Remove from hand
        enemyHand.RemoveAt(randomIndex);

        Debug.Log($"Enemy chose: {chosenCard.Name}");
        return chosenCard;
    }
}