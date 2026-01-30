using UnityEngine;

public class ExecuteStep : IDamagePipelineStep
{
    private bool used = false;

    public int Process(int damage, Character caster, Character target)
    {
        used = true;
        float hpPercent = (float)target.currentHp / target.MaxHp;
        if (hpPercent < 0.5f)
        {
            return damage * 2;
        }
        return damage;
    }

    public bool expired => used;
}