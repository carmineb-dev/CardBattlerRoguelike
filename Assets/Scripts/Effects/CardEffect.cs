using UnityEngine;

public abstract class CardEffect : ScriptableObject
{
    public abstract void Execute(Character caster, Character target, int value);
}