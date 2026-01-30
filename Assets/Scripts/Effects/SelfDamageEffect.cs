using UnityEngine;

[CreateAssetMenu(fileName = "SelfDamageEffect", menuName = "Card Effects/Self Damage")]
public class SelfDamageEffect : CardEffect
{
    public override void Execute(Character caster, Character target, int value)
    {
        caster.TakePiercingDamage(value);
    }
}