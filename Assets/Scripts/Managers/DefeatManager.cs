using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DefeatManager : MonoBehaviour
{
    public static DefeatManager Instance;

    [SerializeField] private GameObject defeatPanel;
    [SerializeField] private TextMeshProUGUI fightReachedText;
    [SerializeField] private Button restartButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Setup restart button
        restartButton.onClick.AddListener(OnRestartClicked);
    }

    public void ShowDefeat()
    {
        int currentFight = GameManager.Instance.GetCurrentFight();
        fightReachedText.text = $"Reached Fight {currentFight}/6";

        defeatPanel.SetActive(true);

        Debug.Log("Defeat Screen Shown");
    }

    private void OnRestartClicked()
    {
        // Reload scene
        SceneManager.LoadScene(1);
    }
}