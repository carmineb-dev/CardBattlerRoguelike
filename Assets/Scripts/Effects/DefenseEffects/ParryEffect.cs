using UnityEngine;

[CreateAssetMenu(fileName = "ParryEffect", menuName = "Card Effects/Parry")]
public class ParryEffect : CardEffect
{
    public override void Execute(Character caster, Character target, int value)
    {
        caster.GainBlock(3);

        // Priority boost for the turn
        caster.priorityBoostRemaining = 99;
        caster.priorityBoostValue = 1;
        caster.hasParryActive = true;
        caster.parryActivatedThisTurn = true;
    }
}