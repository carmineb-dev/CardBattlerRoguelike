using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "PiercingDamageEffect", menuName = ("Card Effects/Piercing Damage"))]
public class PiercingDamage : CardEffect
{
    public override void Execute(Character caster, Character target, int value)
    {
        target.TakePiercingDamage(value);
    }
}