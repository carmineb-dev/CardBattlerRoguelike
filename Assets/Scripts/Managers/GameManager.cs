using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Progression")]
    [SerializeField] private int currentFight = 1;

    [SerializeField] private int totalFights = 6;

    [Header("Enemy Scaling")]
    [SerializeField] private int baseEnemyHP = 20;

    [SerializeField] private int hpScalingPerFight = 5;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetCurrentFight() => currentFight;

    public int GetTotalFights() => totalFights;

    public bool IsBossFight() => currentFight == totalFights;

    public int GetEnemyMaxHP()
    {
        if (IsBossFight())
        {
            return 100;
        }

        // 20 -> 25 -> 30 -> 35 -> 40
        return baseEnemyHP + (currentFight - 1) * hpScalingPerFight;
    }

    public void NextFight()
    {
        currentFight++;
        Debug.Log($"Next fight: {currentFight}/{totalFights}");
    }

    public void ResetProgression()
    {
        currentFight = 1;
        Debug.Log("Progresion reset");
    }
}