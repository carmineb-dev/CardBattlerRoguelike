using UnityEngine;

[CreateAssetMenu(fileName = "DrawEffect", menuName = "Card Effects/Draw")]
public class DrawEffect : CardEffect
{
    public override void Execute(Character caster, Character target, int value)
    {
        // Player draw
        if (caster is Player player)
        {
            // Track extra draws
            player.extraDrawsThisTurn += value;
            Debug.Log($"Player drew {value} cards - extra draws this turn :{player.extraDrawsThisTurn}");
        }
        else
        {
            Enemy enemy = caster as Enemy;
            if (enemy != null)
            {
                enemy.DrawCards(value);
            }
        }
    }
}