using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    private CardData cardData;

    private Character owner; // Who owns this card

    // === CARD UI ===
    [SerializeField] private TextMeshProUGUI nameText;

    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI priorityText;
    [SerializeField] private Image background;

    private Vector3 originalScale;
    private CardData data;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    public void Initialize(CardData data, Character cardOwner)
    {
        cardData = data;
        owner = cardOwner;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (cardData == null)
        {
            return;
        }

        // Color by category
        switch (cardData.Category)
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

        nameText.text = cardData.Name;
        descriptionText.text = cardData.Description;
        costText.text = cardData.Cost.ToString();
        priorityText.text = cardData.Priority.ToString();
    }

    // === CLICK HANDLING ===
    public void OnPointerDown(PointerEventData eventData)
    {
        PlayCard();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale * 1.2f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }

    private void PlayCard()
    {
        // Caster is the owner of the card
        Character caster = owner;

        // Target is the opposite of the caster
        Character target = GetOpponent(caster);

        // Check mana cost
        if (caster.currentMana >= cardData.Cost)
        {
            // Spend mana
            caster.SpendMana(cardData.Cost);

            // Execute card effect
            cardData.Effect.Execute(caster, target, cardData.Value);

            // Remove from hand if player
            if (owner == CombatManager.Instance.Player)
            {
                Hand.Instance.RemoveCard(this);
            }

            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"Not enough mana to play {cardData.Name}");
        }
    }

    private Character GetOpponent(Character character)
    {
        if (character == CombatManager.Instance.Player)
        {
            return CombatManager.Instance.Enemy;
        }
        else
        {
            return CombatManager.Instance.Player;
        }
    }
}