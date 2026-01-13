using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Scriptable Objects/CardData")]
public class CardData : ScriptableObject
{
    [SerializeField] private string cardName;
    [SerializeField] private string cardDescription;
    [SerializeField] private int cardCost;
    [SerializeField] private int cardPriority;

    public string Name => cardName;
    public string Description => cardDescription;
    public int Cost => cardCost;
    public int Priority => cardPriority;
}