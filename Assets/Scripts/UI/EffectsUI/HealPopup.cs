using System.Collections;
using TMPro;
using UnityEngine;

public class HealPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI healText;
    [SerializeField] private float duration = 1f;

    public void ShowHeal(int amount)
    {
        StartCoroutine(ShowHealRoutine(amount));
    }

    private IEnumerator ShowHealRoutine(int amount)
    {
        healText.text = $"+{amount}";
        healText.gameObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        healText.gameObject.SetActive(false);
    }
}