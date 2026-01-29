using JetBrains.Annotations;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

public class Player : Character
{
    // === UI ===
    [SerializeField] private PlayerHealthUI healthUI;

    [SerializeField] private PlayerManaUI manaUI;
    [SerializeField] private PlayerBlockUI blockUI;

    [SerializeField] private Hand hand;
    public Hand Hand => hand;

    private void Awake()
    {
        characterName = "Player";
    }

    // === INITIALIZE ===
    public override void Initialize()
    {
        // Stats
        currentHp = maxHp;
        maxMana = baseMaxMana;
        currentMana = maxMana;
        currentBlock = 0;

        // UI
        UpdateAllUI();
    }

    // === DAMAGE ===
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        blockUI.UpdateUI(currentBlock);
        healthUI.UpdateUI(currentHp);
    }

    protected override void Die()
    {
        base.Die();
        Debug.Log("Game Over");
    }

    // === BLOCK ===
    public override void GainBlock(int amount)
    {
        base.GainBlock(amount);
        blockUI.UpdateUI(currentBlock);
    }

    public override void ResetBlock()
    {
        base.ResetBlock();
        blockUI.UpdateUI(currentBlock);
    }

    // === MANA ===

    public override void SpendMana(int amount)
    {
        base.SpendMana(amount);
        manaUI.UpdateUI(currentMana, maxMana);
    }

    public override void RefillMana()
    {
        base.RefillMana();
        manaUI.UpdateUI(currentMana, maxMana);
    }

    public override void IncreaseMaxMana(int amount)
    {
        base.IncreaseMaxMana(amount);
        manaUI.UpdateUI(currentMana, maxMana);
    }

    // === HEAL ===
    public override void Heal(int amount)
    {
        base.Heal(amount);
        healthUI.UpdateUI(currentHp);
    }

    private void UpdateAllUI()
    {
        healthUI.UpdateUI(currentHp);
        manaUI.UpdateUI(currentMana, maxMana);
        blockUI.UpdateUI(currentBlock);
    }
}