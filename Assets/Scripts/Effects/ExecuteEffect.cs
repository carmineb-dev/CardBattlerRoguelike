using UnityEngine;

[CreateAssetMenu(fileName = "ExecuteEffect", menuName = "Card Effects/Execute")]
public class ExecuteEffect : CardEffect
{
    public override void Execute(Character caster, Character target, int value)
    {
        int damage;
        float hpPercentage = (float)target.currentHp / target.MaxHp;

        if (hpPercentage < 0.5f)
        {
            damage = 10;
        }
        else
        {
            damage = 5;
        }

        target.TakeDamage(damage);
    }
}