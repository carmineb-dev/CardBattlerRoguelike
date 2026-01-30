using UnityEngine;

[CreateAssetMenu(fileName = "ManaCostEffect", menuName = "Card Effects/ManaCost")]
public class ManaCostEffect : CardEffect
{
    public override void Execute(Character caster, Character target, int value)
    {
        caster.nextCardFree = true;
        Debug.Log($"{caster.characterName} next card costs 0 mana!");
    }
}