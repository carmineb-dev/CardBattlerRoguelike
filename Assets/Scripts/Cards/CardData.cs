using System.Runtime.CompilerServices;
using UnityEngine;

public enum CardCategory
{ Attack, Defense, Utility, Advanced }

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardData")]
public class CardData : ScriptableObject
{
    [SerializeField] private string cardName;
    [SerializeField] private string cardDescription;
    [SerializeField] private int cardCost;
    [SerializeField] private int cardPriority;
    [SerializeField] private CardCategory cardCategory;
    [SerializeField] private CardEffect effect;
    [SerializeField] private int value; // value of the effect
    public string Name => cardName;

    public string Description => cardDescription;

    public int Cost => cardCost;

    public int Priority => cardPriority;

    public CardCategory Category => cardCategory;

    public CardEffect Effect => effect;

    public int Value => value;
}