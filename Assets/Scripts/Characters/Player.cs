using JetBrains.Annotations;
using UnityEngine;

public class Player : MonoBehaviour
{
    // === STATS ===
    [SerializeField] private int maxHp = 50;

    [SerializeField] private int maxMana = 5;
    [SerializeField] private int currentHp;
    [SerializeField] private int currentMana;
    [SerializeField] private int block;

    // === UI ===
    [SerializeField] private PlayerHealthUI healthUI;

    [SerializeField] private PlayerManaUI manaUI;
    [SerializeField] private PlayerBlockUI blockUI;

    public void TakeDamage(int damage)
    {
        int remainingDamage = damage;

        if (block > 0)
        {
            int absorbedDamage = Mathf.Min(block, remainingDamage);
            block -= absorbedDamage;
            remainingDamage -= absorbedDamage;

            blockUI.UpdateUI(block);
        }

        if (remainingDamage > 0)
        {
            currentHp = Mathf.Clamp(currentHp - remainingDamage, 0, maxHp);
            healthUI.UpdateUI(currentHp);
        }

        Debug.Log($"Player HP: {currentHp}");
    }

    public void GainBlock(int amount)
    {
        block += amount;
        blockUI.UpdateUI(block);
        Debug.Log($"Player Block: {block}");
    }

    public bool canSpendMana(int amount)
    {
        return currentMana >= amount;
    }

    public void SpendMana(int amount)
    {
        currentMana = Mathf.Clamp(currentMana - amount, 0, maxMana);
        manaUI.UpdateUI(currentMana, maxMana);
    }

    public void RefillMana()
    {
        currentMana = maxMana;
        manaUI.UpdateUI(currentMana, maxMana);
    }

    public void Initialize()
    {
        // Stats
        currentHp = maxHp;
        currentMana = maxMana;
        block = 0;

        // UI
        healthUI.UpdateUI(currentHp);
        manaUI.UpdateUI(currentMana, maxMana);
    }
}