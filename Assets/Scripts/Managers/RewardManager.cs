using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RewardManager : MonoBehaviour
{
    public static RewardManager Instance;

    [Header("UI References")]
    [SerializeField] private GameObject rewardPanel;

    [SerializeField] private Button rewardButton1;
    [SerializeField] private Button rewardButton2;
    [SerializeField] private Button rewardButton3;
    [SerializeField] private Button nextFightButton;

    [SerializeField] private TextMeshProUGUI reward1Text;
    [SerializeField] private TextMeshProUGUI reward2Text;
    [SerializeField] private TextMeshProUGUI reward3Text;

    [Header("Card Pool")]
    [SerializeField] private List<CardData> cardRewardPool;

    private CardData reward1Card;
    private CardData reward2Card;
    private CardData reward3Card;

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

        // Next fight button listener
        nextFightButton.onClick.AddListener(OnNextFightClicked);
    }

    public void ShowRewards()
    {
        Debug.Log("ShowRewards CALLED");
        // Generate 3 random rewards
        GenerateRewards();

        // Show Panel
        rewardPanel.SetActive(true);

        // Setup buttons
        rewardButton1.onClick.RemoveAllListeners();
        rewardButton1.onClick.AddListener(OnReward1Clicked);

        rewardButton2.onClick.RemoveAllListeners();
        rewardButton2.onClick.AddListener(OnReward2Clicked);

        rewardButton3.onClick.RemoveAllListeners();
        rewardButton3.onClick.AddListener(OnReward3Clicked);
    }

    private void OnReward1Clicked()
    {
        ChooseCardReward(reward1Card);
    }

    private void OnReward2Clicked()
    {
        ChooseStatReward();
    }

    private void OnReward3Clicked()
    {
        ChooseCardReward(reward3Card);
    }

    private void GenerateRewards()
    {
        // Reward 1: Random card
        reward1Card = cardRewardPool[Random.Range(0, cardRewardPool.Count)];
        reward1Text.text = $"{reward1Card.Name}\n{reward1Card.Description}";

        // Reward 2: Stat upgrade (+1 mana)
        reward2Text.text = "+1 Max Mana";

        // Reward 3: Random card different from reward 1
        do
        {
            reward3Card = cardRewardPool[Random.Range(0, cardRewardPool.Count)];
        } while (reward3Card == reward1Card);
        reward3Text.text = $"{reward3Card.Name}\n{reward3Card.Description}";
    }

    private void ChooseCardReward(CardData card)
    {
        // Add card to player deck
        Deck.Instance.AddCardToDeck(card);

        Debug.Log($"Reward chosen: {card.Name} added to deck");

        // Hide reward buttons
        rewardButton1.gameObject.SetActive(false);
        rewardButton2.gameObject.SetActive(false);
        rewardButton3.gameObject.SetActive(false);

        // Show next fight button
        nextFightButton.gameObject.SetActive(true);
    }

    private void ChooseStatReward()
    {
        // Increase max mana permanentely
        CombatManager.Instance.Player.IncreaseBaseMaxMana(1);

        Debug.Log("Reward chosen: +1 max mana");

        // Hide reward buttons
        rewardButton1.gameObject.SetActive(false);
        rewardButton2.gameObject.SetActive(false);
        rewardButton3.gameObject.SetActive(false);

        // Show next fight button
        nextFightButton.gameObject.SetActive(true);
    }

    private void OnNextFightClicked()
    {
        // Reset UI
        rewardButton1.gameObject.SetActive(true);
        rewardButton2.gameObject.SetActive(true);
        rewardButton3.gameObject.SetActive(true);

        // Hide panel
        rewardPanel.SetActive(false);
        nextFightButton.gameObject.SetActive(false);

        // Continue
        CombatManager.Instance.ContinueToNextFight();
    }
}