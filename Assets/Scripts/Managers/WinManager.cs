using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinManager : MonoBehaviour
{
    public static WinManager Instance;

    [SerializeField] private GameObject winPanel;
    [SerializeField] private Button playAgainButton;

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

        playAgainButton.onClick.AddListener(OnPlayAgainClicked);
    }

    public void ShowWin()
    {
        winPanel.SetActive(true);
        Debug.Log("Win screen shown - Boss defeated!");
    }

    private void OnPlayAgainClicked()
    {
        SceneManager.LoadScene(1);
    }
}