using UnityEngine;

[CreateAssetMenu(fileName = "TimeWarpEffect", menuName = "Card Effects/TimeWarp")]
public class TimeWarpEffect : CardEffect
{
    public override void Execute(Character caster, Character target, int value)
    {
        target.skipTurns = value;
        Debug.Log($"{target.characterName} will skip next {value} turn!");
    }
}