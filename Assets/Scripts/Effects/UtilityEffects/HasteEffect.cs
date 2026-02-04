using UnityEngine;

[CreateAssetMenu(fileName = "HasteEffect", menuName = "Card Effects/Haste")]
public class HasteEffect : CardEffect
{
    public override void Execute(Character caster, Character target, int value)
    {
        // Boost next 2 cards
        caster.priorityBoostRemaining = 2;
        caster.priorityBoostValue = 1;

        Debug.Log($"{caster.characterName} Haste: next 2 cards have -1 priority");
    }
}