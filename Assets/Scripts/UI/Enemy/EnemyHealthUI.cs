using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthUI : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;

    public void InitializeUI(int health)
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;
        healthText.text = $"{health}/{health}";
    }

    public void UpdateUI(int currentHealth, int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthText.text = $"{currentHealth}/{maxHealth}";
        healthSlider.value = currentHealth;
    }
}