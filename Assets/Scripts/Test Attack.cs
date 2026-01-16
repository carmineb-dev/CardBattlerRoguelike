using UnityEngine;

public class TestAttack : MonoBehaviour
{
    public Player player;
    public Enemy enemy;

    public void Spendmana()
    {
        player.SpendMana(2);
    }

    public void dmg()
    {
        player.TakeDamage(5);
    }

    public void refillmana()
    {
        player.RefillMana();
    }

    public void gainblock()
    {
        player.GainBlock(6);
    }

    public void enemydmg()
    {
        enemy.TakeDamage(12);
    }
}