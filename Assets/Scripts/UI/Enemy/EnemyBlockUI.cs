using UnityEngine;

using TMPro;

public class EnemyBlockUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemyBlockText;
    [SerializeField] private GameObject enemyBlockImage;

    public void UpdateUI(int currentBlock)
    {
        if (currentBlock == 0)
        {
            enemyBlockImage.SetActive(false);
        }
        else if (currentBlock > 0)
        {
            enemyBlockImage.SetActive(true);
        }

        enemyBlockText.text = currentBlock.ToString();
    }
}