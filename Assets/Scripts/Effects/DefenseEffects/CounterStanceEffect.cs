using UnityEngine;

[CreateAssetMenu(fileName = "CounterStance", menuName = "Card Effects/Counter Stance")]
public class CounterStanceEffect : CardEffect
{
    public override void Execute(Character caster, Character target, int value)
    {
        caster.GainBlock(4);
        caster.hasCounterStance = true;
        caster.counterStanceDamage = value;
    }
}