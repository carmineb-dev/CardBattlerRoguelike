using UnityEngine;

[CreateAssetMenu(fileName = "DamageEffect", menuName = "Card Effects/Damage")]
public class DamageEffect : CardEffect
{
    public override void Execute(Character caster, Character target, int value)
    {
        int finalDamage = value;
        if (caster.isDamageMultiplierActive)
        {
            finalDamage *= caster.damageMultiplier;
            caster.isDamageMultiplierActive = false;
        }
        target.TakeDamage(finalDamage);
    }
}