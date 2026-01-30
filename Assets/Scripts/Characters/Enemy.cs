using UnityEngine;

public class Enemy : Character
{
    // === UI ===
    [SerializeField] private EnemyHealthUI healthUI;

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
}