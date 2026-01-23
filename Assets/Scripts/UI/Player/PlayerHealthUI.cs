using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerHealthText;

    public void UpdateUI(int currentHP)
    {
        playerHealthText.text = currentHP.ToString();
    }
}