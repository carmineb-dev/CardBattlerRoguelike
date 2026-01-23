using UnityEngine;

[CreateAssetMenu(fileName = "CompositeEffect", menuName = "Card Effects/Composite")]
public class CompositeEffect : CardEffect
{
    [System.Serializable]
    public struct SubEffects
    {
        public CardEffect effect;
        public int value;
    }

    public SubEffects[] subEffects;

    public override void Execute(Character caster, Character target, int unusedValue)
    {
        foreach (var entry in subEffects)
        {
            entry.effect.Execute(caster, target, entry.value);
        }
    }
}