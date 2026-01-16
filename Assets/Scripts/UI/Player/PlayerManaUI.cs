using TMPro;
using UnityEngine;

public class PlayerManaUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerManaText;

    public void UpdateUI(int currentMana, int maxMana)
    {
        playerManaText.text = $"{currentMana}/{maxMana}";
    }
}