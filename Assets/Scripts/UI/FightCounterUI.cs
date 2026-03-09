using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class FightCounterUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fightText;

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        int current = GameManager.Instance.GetCurrentFight();
        int total = GameManager.Instance.GetTotalFights();

        if (GameManager.Instance.IsBossFight())
        {
            fightText.text = "BOSS FIGHT";
            fightText.color = Color.red;
        }
        else
        {
            fightText.text = $"Fight {current}/{total}";
            fightText.color = Color.white;
        }
    }
}