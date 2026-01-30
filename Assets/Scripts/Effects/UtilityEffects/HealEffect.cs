using UnityEngine;

[CreateAssetMenu(fileName = "HealEffect", menuName = "Card Effects/Heal")]
public class HealEffect : CardEffect
{
    public override void Execute(Character caster, Character target, int value)
    {
        caster.Heal(value);
    }
}