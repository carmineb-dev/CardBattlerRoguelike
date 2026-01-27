using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public string characterName;

    // === STATS ===
    [Header("Stats")]
    [SerializeField] protected int maxHp;

    public int MaxHp => maxHp;

    [SerializeField] protected int maxMana;
    public int currentHp { get; protected set; }

    public int currentMana { get; protected set; }

    public int currentBlock { get; protected set; }

    // === BUFFS ===

    public bool negateNextAttck = false;
    public bool nextCardFree = false;
    [SerializeField] private HealPopup healPopup;

    public abstract void Initialize();

    // === DAMAGE ===
    public virtual void TakeDamage(int damage)
    {
        if (negateNextAttck)
        {
            Debug.Log($"{characterName} negated attack!");
            negateNextAttck = false;
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
}