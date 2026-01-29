using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "NegateEffect", menuName = ("Card Effects/Negate"))]
public class NegateEffect : CardEffect
{
    public override void Execute(Character caster, Character target, int value)
    {
        caster.negateNextAttack = true;
        Debug.Log($"{caster.characterName} will negate next attack");
    }
}