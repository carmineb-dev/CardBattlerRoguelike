using UnityEngine;

public abstract class Character : MonoBehaviour
{
    public string characterName;

    [SerializeField] private HealPopup healPopup;

    [Header("Stats")]
    [SerializeField] protected int maxHp;

    [SerializeField] protected int maxMana;
    public int currentHp { get; protected set; }

    public int currentMana { get; protected set; }

    public int currentBlock { get; protected set; }

    public abstract void Initialize();

    public virtual void TakeDamage(int damage)
    {
        int damageAfterBlock = Mathf.Max(0, damage - currentBlock);
        currentBlock = Mathf.Max(0, currentBlock - damage);
        currentHp -= damageAfterBlock;

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