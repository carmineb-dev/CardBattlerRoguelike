using UnityEngine;

[CreateAssetMenu(fileName = "ExecuteEffect", menuName = "Card Effects/Execute")]
public class ExecuteEffect : CardEffect
{
    public override void Execute(Character caster, Character target, int value)
    {
        float hpPercentage = (float)target.currentHp / target.MaxHp;

        if (caster.isDamageMultiplierActive)
        {
            value *= caster.damageMultiplier;
            caster.isDamageMultiplierActive = false;
        }

        if (hpPercentage < 0.5f)
        {
            value *= 2;
        }

        target.TakeDamage(value);
    }
}