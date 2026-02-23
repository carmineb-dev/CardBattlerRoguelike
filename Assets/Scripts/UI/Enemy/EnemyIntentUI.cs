using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EnemyIntentUI : MonoBehaviour
{
    [SerializeField] private Image intentIcon;
    [SerializeField] private Sprite attackIcon;
    [SerializeField] private Sprite defendIcon;
    [SerializeField] private Sprite utilityIcon;

    public void ShowIntent(List<CardData> cards)
    {
        if (cards.Count == 0)
        {
            intentIcon.gameObject.SetActive(false);
            return;
        }

        // Define dominant intent
        int attackCount = 0;
        int defendCount = 0;
        int utilityCount = 0;

        foreach (CardData card in cards)
        {
            if (card.Category == CardCategory.Attack)
            {
                attackCount++;
            }
            else if (card.Category == CardCategory.Defense)
            {
                defendCount++;
            }
            else
            {
                utilityCount++;
            }
        }

        // Show the most representative
        if (attackCount > defendCount && attackCount > utilityCount)
        {
            intentIcon.sprite = attackIcon;
        }
        else if (defendCount > attackCount && defendCount > utilityCount)
        {
            intentIcon.sprite = defendIcon;
        }
        else
        {
            intentIcon.sprite = utilityIcon;
        }

        intentIcon.gameObject.SetActive(true);
    }

    public void HideIntent()
    {
        intentIcon.gameObject.SetActive(false);
    }
}