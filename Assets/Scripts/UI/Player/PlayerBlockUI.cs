using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBlockUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerBlockText;
    [SerializeField] private GameObject playerBlockImage;

    public void UpdateUI(int currentBlock)
    {
        if (currentBlock == 0)
        {
            playerBlockImage.SetActive(false);
        }
        else if (currentBlock > 0)
        {
            playerBlockImage.SetActive(true);
        }

        playerBlockText.text = currentBlock.ToString();
    }
}