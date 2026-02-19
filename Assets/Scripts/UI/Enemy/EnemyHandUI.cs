using UnityEngine;
using TMPro;

public class EnemyHandUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI handCountText;

    public void UpdateHandCount(int count)
    {
        handCountText.text = $"Cards: {count}";
    }
}