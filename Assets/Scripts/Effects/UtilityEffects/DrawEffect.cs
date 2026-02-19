using UnityEngine;

[CreateAssetMenu(fileName = "DrawEffect", menuName = "Card Effects/Draw")]
public class DrawEffect : CardEffect
{
    public override void Execute(Character caster, Character target, int value)
    {
        // Player draw
        if (caster == CombatManager.Instance.Player)
        {
            for (int i = 0; i < value; i++)
            {
                Deck.Instance.DrawCard();
            }
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