using UnityEngine;

public class EndTurn : MonoBehaviour
{
    public void endTurn()
    {
        CombatManager.Instance.EndTurn();
    }
}