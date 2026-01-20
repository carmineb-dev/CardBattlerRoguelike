using UnityEngine;

[CreateAssetMenu(fileName = "BlockEffect", menuName = "Card Effects/Block")]
public class BlockEffect : CardEffect
{
    public override void Execute(Character caster, Character target, int value)
    {
        caster.GainBlock(value);
    }
}