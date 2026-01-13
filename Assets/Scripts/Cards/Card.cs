using System.Data.Common;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [SerializeField] private CardData cardData;

    // === CARD UI ===
    [SerializeField] private TextMeshProUGUI nameText;

    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI priorityText;
    [SerializeField] private Image background;

    private CardData data;

    /*
    private void Start()
    {
        Initialize(cardData);
        UpdateVisual();
    }
    */

    public void Initialize(CardData cardData)
    {
        data = cardData;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (data == null)
        {
            return;
        }

        switch (data.Category)
        {
            case CardCategory.Attack:
                background.color = Color.red;
                break;

            case CardCategory.Defense:
                background.color = Color.blue;
                break;

            case CardCategory.Utility:
                background.color = Color.green;
                break;

            case CardCategory.Advanced:
                background.color = Color.yellow;
                break;
        }

        nameText.text = data.Name;
        descriptionText.text = data.Description;
        costText.text = data.Cost.ToString();
        priorityText.text = data.Priority.ToString();
    }

    public void SetData(CardData data)
    {
        cardData = data;
        UpdateVisual();
    }
}