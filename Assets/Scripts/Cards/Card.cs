using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField] private CardData cardData;

    // === CARD UI ===
    [SerializeField] private TextMeshProUGUI nameText;

    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI priorityText;

    private void Start()
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (cardData == null)
        {
            return;
        }

        nameText.text = cardData.Name;
        descriptionText.text = cardData.Description;
        costText.text = cardData.Cost.ToString();
        priorityText.text = cardData.Priority.ToString();
    }

    public void SetData(CardData data)
    {
        cardData = data;
        UpdateVisual();
    }
}