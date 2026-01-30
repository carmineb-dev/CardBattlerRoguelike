using UnityEngine;

[CreateAssetMenu(fileName = "FuryEffect", menuName = "Card Effects/Fury")]
public class FuryEffect : CardEffect
{
    public override void Execute(Character caster, Character target, int value)
    {
        caster.ActivateFuryNextAttack();
    }
}