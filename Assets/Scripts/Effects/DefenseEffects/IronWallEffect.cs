using UnityEngine;

[CreateAssetMenu(fileName = "IronWallEffect", menuName = "Card Effects/IronWall")]
public class IronWallEffect : CardEffect
{
    public override void Execute(Character caster, Character target, int value)
    {
        // Immediate Block
        caster.GainBlock(8);

        // Regen block for 3 turns
        caster.ironWallBlockPerTurn = 2;
        caster.ironWallTurns = 3;

        Debug.Log($"{caster.characterName} gained 8 block + 2 block/turn for 3 turns");
    }
}