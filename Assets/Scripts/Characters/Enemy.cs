using UnityEngine;

public class Enemy : MonoBehaviour
{
    // === STATS ===
    [SerializeField] private int maxHp = 30;

    [SerializeField] private int currentHp;

    // === UI ===
    [SerializeField] private EnemyHealthUI healthUI;

    public void TakeDamage(int damage)
    {
        currentHp = Mathf.Clamp(currentHp - damage, 0, maxHp);
        healthUI.UpdateUI(currentHp, maxHp);

        Debug.Log($"Enemy HP: {currentHp}");

        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy defeated!");
    }

    public void Initialize()
    {
        // Stats
        currentHp = maxHp;

        // UI
        healthUI.InitializeUI(maxHp);
    }
}