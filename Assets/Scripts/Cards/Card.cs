using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.LowLevelPhysics2D;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public CardData cardData { get; private set; }

    private Character owner; // Who owns this card

    // === CARD UI ===
    [SerializeField] private TextMeshProUGUI nameText;

    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI costText;
    [SerializeField] private TextMeshProUGUI priorityText;
    [SerializeField] private Image background;

    private bool isPlayed;

    private Vector3 originalScale;

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

        // Calculate final priority
        int finalPriority = cardData.Priority;

        // Apply priority boost if possible
        if (caster.priorityBoostRemaining > 0)
        {
            finalPriority -= caster.priorityBoostValue;

            // Decrement only if is not active parry card effect
            if (!caster.hasParryActive)
            {
                caster.priorityBoostRemaining--;
            }

            // Visual feedback
            priorityText.color = Color.yellow;
            priorityText.text = finalPriority.ToString();
            Debug.Log($"{cardData.Name} boosted: {cardData.Priority}->{finalPriority}");
        }

        // Check mana cost
        if (caster.currentMana >= cardData.Cost)
        {
            if (caster.nextCardFree)
            {
                // Don't spend mana
                caster.nextCardFree = false;
                Debug.Log("Card played for free!");
            }
            else
            {   // Spend mana
                caster.SpendMana(cardData.Cost);
            }

            // Queue card effect
            CombatManager.Instance.QueueCard(this, caster, target, finalPriority);

            isPlayed = true;

            // Visual feedback of the played card
            CanvasGroup cg = GetComponent<CanvasGroup>();
            cg.interactable = false; // Not clickable anymore
            cg.blocksRaycasts = false;

            // Slide animation
            StartCoroutine(SlideToCenter());
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

    public bool IsPlayed()
    {
        return isPlayed;
    }

    // === SLIDE ANIMATION ===
    private IEnumerator SlideToCenter()
    {
        transform.SetAsLastSibling();

        Vector3 startPos = transform.position;

        // Choose position
        float xPos = Screen.width * 0.2f;
        float yPos = Screen.height / 2f;
        Vector3 targetPos = new Vector3(xPos, yPos, startPos.z);

        float duration = 0.3f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Slide position
            transform.position = Vector3.Lerp(startPos, targetPos, t);

            yield return null;
        }
        transform.position = targetPos;

        Canvas mainCanvas = GetComponentInParent<Canvas>();
        if (mainCanvas != null)
        {
            transform.SetParent(mainCanvas.transform, true);
        }
    }

    // === FADE OUT ===
    public IEnumerator FadeOutAndDestroy()
    {
        Debug.Log($"FadeOut START: {cardData.Name}");
        CanvasGroup cg = GetComponent<CanvasGroup>();

        float duration = 0.3f;
        float elapsed = 0f;
        float startAlpha = cg.alpha;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / duration);
            yield return null;
        }
        Debug.Log($"FadeOut COMPLETE: {cardData.Name}");
        Destroy(gameObject);
    }

    public void TriggerFadeOut()
    {
        StartCoroutine(FadeOutAndDestroy());
    }
}