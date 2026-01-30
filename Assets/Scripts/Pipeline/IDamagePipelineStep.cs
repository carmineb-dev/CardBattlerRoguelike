using UnityEngine;

public interface IDamagePipelineStep
{
    int Process(int damage, Character caster, Character target);

    bool expired { get; }
}