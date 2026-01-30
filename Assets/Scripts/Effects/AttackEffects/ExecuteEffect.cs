using UnityEngine;

[CreateAssetMenu(fileName = "ExecuteEffect", menuName = "Card Effects/Execute")]
public class ExecuteEffect : CardEffect
{
    public override void Execute(Character caster, Character target, int value)
    {
        caster.AddDamageStep(new ExecuteStep());
        caster.DealDamage(target, value);
    }
}