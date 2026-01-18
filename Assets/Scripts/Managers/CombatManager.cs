using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance;

    // === References ===
    [SerializeField] private Player player;

    public Player Player => player;

    [SerializeField] private Enemy enemy;
    public Enemy Enemy => enemy;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        InitializeCombat();
    }

    private void InitializeCombat()
    {
        player.Initialize();
        enemy.Initialize();
    }
}