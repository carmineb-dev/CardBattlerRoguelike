using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public string characterName;

    // === STATS ===
    [Header("Stats")]
    [SerializeField] protected int maxHp;

    public int MaxHp => maxHp;

    [SerializeField] protected int maxMana; // max mana value in combat
    [SerializeField] protected int baseMaxMana; // max mana value out of the combat
    public int currentHp { get; protected set; }

    public int currentMana { get; protected set; }

    public int currentBlock { get; protected set; }

    // === BUFFS ===

    public bool negateNextAttack = false;
    public bool nextCardFree = false;
    public bool hasFuryNextAttack = false;
    public int damageMultiplier = 1;
    [SerializeField] private HealPopup healPopup;

    // === TURN BASED BUFFS ===
    [Header("Turn-Based Buffs")]
    public int ironWallTurns = 0; // Iron Wall

    public int ironWallBlockPerTurn = 0;

    public int skipTurns = 0; // Time Warp

    public int priorityBoostRemaining = 0; // How many cards left
    public int priorityBoostValue = 0; // How much boost (-1 = faster)
    public bool hasParryActive = false;
    public bool parryActivatedThisTurn = false;

    // === COUNTER STANCE ===
    public bool hasCounterStance = false;

    public int counterStanceDamage = 0;
    public Character lastAttacker;

    public abstract void Initialize();

    // === DAMAGE PIPELINE ===
    private readonly List<IDamagePipelineStep> damagePipeline = new();

    // === DAMAGE ===
    public virtual void TakeDamage(int damage)
    {
        // Counter stance trigger
        if (hasCounterStance && lastAttacker != null)
        {
            Debug.Log("Counter stance triggered!");

            DealDamage(lastAttacker, counterStanceDamage);
            hasCounterStance = false;
        }

        if (negateNextAttack)
        {
            Debug.Log($"{characterName} negated attack!");
            negateNextAttack = false;
            return;
        }

        int damageAfterBlock = Mathf.Max(0, damage - currentBlock);
        currentBlock = Mathf.Max(0, currentBlock - damage);
        currentHp = Mathf.Clamp(currentHp - damageAfterBlock, 0, maxHp);

        Debug.Log($"{characterName} HP: {currentHp}");

        if (currentHp <= 0)
        {
            Die();
        }
    }

    public virtual void TakePiercingDamage(int damage)
    {
        currentHp = Mathf.Clamp(currentHp - damage, 0, maxHp);
    }

    protected virtual void Die()
    {
        Debug.Log($"{name} died");
    }

    // === MANA ===
    public virtual void SpendMana(int amount)
    {
        currentMana = Mathf.Clamp(currentMana - amount, 0, maxMana);
    }

    public virtual void RefillMana()
    {
        currentMana = maxMana;
    }

    public virtual void IncreaseMaxMana(int amount)
    {
        maxMana += amount;
    }

    // === BLOCK ===
    public virtual void GainBlock(int amount)
    {
        currentBlock += amount;
    }

    public virtual void ResetBlock()
    {
        currentBlock = 0;
    }

    // === HEAL ===
    public virtual void Heal(int amount)
    {
        int hpBefore = currentHp;
        currentHp = Mathf.Clamp(currentHp + amount, 0, maxHp);
        int healedHp = currentHp - hpBefore;

        if (healedHp > 0)
        {
            healPopup.ShowHeal(healedHp);
        }

        Debug.Log($"{characterName} healed {amount} HP. Current {currentHp}/{maxHp}");
    }

    // === PIPELINE METHODS ===

    public void AddDamageStep(IDamagePipelineStep step)
    {
        damagePipeline.Add(step);
    }

    public void DealDamage(Character target, int baseDamage)
    {
        int damage = baseDamage;

        if (hasFuryNextAttack)
        {
            damage *= 2;
            ResetFury();
        }

        foreach (var step in damagePipeline)
        {
            damage = step.Process(damage, this, target);
        }

        // Memorize the attacker
        target.lastAttacker = this;

        target.TakeDamage(damage);
        CleanupPipeline();
    }

    private void CleanupPipeline()
    {
        damagePipeline.RemoveAll(step => step.expired);
    }

    // === FURY METHODS ===
    public void ActivateFuryNextAttack()
    {
        hasFuryNextAttack = true;
    }

    public void ResetFury()
    {
        hasFuryNextAttack = false;
    }
}