using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStrategy
{
    Random,
    Aggressive, // Attack based
    Defensive   // Defense based
}

public class Enemy : Character
{
    // === UI ===
    [SerializeField] private EnemyHealthUI healthUI;

    [SerializeField] private EnemyHandUI handUI;

    // === HAND ===
    private List<CardData> enemyHand = new List<CardData>();

    private List<CardData> preChosenCards = new List<CardData>();

    // === STRATEGY ===
    [SerializeField] private EnemyStrategy strategy = EnemyStrategy.Random;

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

    // === CHOOSE CARDS METHODS ===
    public void PreChooseCardsForTurn()
    {
        // Choose cards before player plays cards
        preChosenCards = ChooseCardsToPlay();
    }

    public List<CardData> GetPreChosenCards()
    {
        return preChosenCards;
    }

    public void ClearPreChosenCards()
    {
        preChosenCards.Clear();
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

            // Choose card based on strategy
            CardData chosenCard = null;

            switch (strategy)
            {
                case EnemyStrategy.Random:
                    chosenCard = ChooseRandom(playableCards);
                    break;

                case EnemyStrategy.Aggressive:
                    chosenCard = ChooseAggressive(playableCards);
                    break;

                case EnemyStrategy.Defensive:
                    chosenCard = ChooseDefensive(playableCards);
                    break;
            }

            if (chosenCard == null)
            {
                break;
            }

            // Add card to play list
            cardsToPlay.Add(chosenCard);

            // Remove from hand
            enemyHand.Remove(chosenCard);

            // Spend mana
            SpendMana(chosenCard.Cost);

            Debug.Log($"Enemy [{strategy} queued: {chosenCard.Name}]");
        }

        Debug.Log($"Enemy chose {cardsToPlay.Count} cards to play");

        // Update UI
        if (handUI != null)
        {
            handUI.UpdateHandCount(enemyHand.Count);
        }

        return cardsToPlay;
    }

    // === STRATEGY METHODS ===
    private CardData ChooseRandom(List<CardData> playableCards)
    {
        int randomIndex = Random.Range(0, playableCards.Count);
        return playableCards[randomIndex];
    }

    private CardData ChooseAggressive(List<CardData> playableCards)
    {
        // Priority: Attack > Utility > Defense

        // If hp < 30% -> emergency defense
        float hpPercent = (float)currentHp / maxHp;

        if (hpPercent < 0.3f)
        {
            // Search defense card
            CardData defenseCard = playableCards.Find(c => c.Category == CardCategory.Defense);
            if (defenseCard != null)
            {
                Debug.Log("Aggressive AI: Emergency defense!");
                return defenseCard;
            }
        }

        // Prefer attack cards
        List<CardData> attackCards = playableCards.FindAll(c => c.Category == CardCategory.Attack);
        if (attackCards.Count > 0)
        {
            return attackCards[Random.Range(0, attackCards.Count)];
        }

        // Fallback: utility
        List<CardData> utilityCards = playableCards.FindAll(c => c.Category == CardCategory.Utility);
        if (utilityCards.Count > 0)
        {
            return utilityCards[Random.Range(0, utilityCards.Count)];
        }

        // Last fallback : random
        return ChooseRandom(playableCards);
    }

    private CardData ChooseDefensive(List<CardData> playableCards)
    {
        // Priority: Balanced, but prefer defense when hp is low

        float hpPercent = (float)currentHp / maxHp;

        // If hp < 50% -> defense priority
        if (hpPercent < 0.5f)
        {
            List<CardData> defenseCards = playableCards.FindAll(c => c.Category == CardCategory.Defense);
            if (defenseCards.Count > 0)
            {
                Debug.Log("Defensive AI: HP low, defending!");
                return defenseCards[Random.Range(0, defenseCards.Count)];
            }
        }

        // Else prefers attack 60% or defense/utility 40%
        float roll = Random.value;

        if (roll < 0.6f)
        {
            // Try attack
            List<CardData> attackCards = playableCards.FindAll(c => c.Category == CardCategory.Attack);
            if (attackCards.Count > 0)
            {
                return attackCards[Random.Range(0, attackCards.Count)];
            }
        }

        // Fallback: random card
        return ChooseRandom(playableCards);
    }

    // === DRAW METHOD ===
    public void DrawToHandSize(int targetSize)
    {
        int cardsToDraw = Mathf.Max(targetSize - enemyHand.Count, 0);

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

        // Update UI
        handUI.UpdateHandCount(enemyHand.Count);
    }

    public void DrawCards(int count)
    {
        for (int i = 0; i < count; i++)
        {
            CardData card = EnemyDeck.Instance.Draw();
            if (card != null)
            {
                enemyHand.Add(card);
            }
        }

        // Update UI
        if (handUI != null)
        {
            handUI.UpdateHandCount(enemyHand.Count);
        }
        Debug.Log($"Enemy drew {count} cards, hand: {enemyHand.Count}");
    }
}