using UnityEngine;

[CreateAssetMenu(fileName = "BuffEffect", menuName = "Card Effects/Buff")]
public class BuffEffect : CardEffect
{
    public enum BuffType
    {
        PriorityBoost,
        DamageMultiplier,
        MaxManaIncrease
    }

    [SerializeField] private BuffType buffType;
    [SerializeField] private int buffValue;
    [SerializeField] private int duration = 1; // -1 = permanent

    public override void Execute(Character caster, Character target, int value)
    {
        switch (buffType)
        {
            case BuffType.MaxManaIncrease:
                caster.IncreaseMaxMana(buffValue);
                Debug.Log($"{caster.characterName} gained +{buffValue} max mana!");
                break;

            case BuffType.DamageMultiplier:
                caster.isDamageMultiplierActive = true;
                caster.damageMultiplier = buffValue;
                break;
        }
    }
}