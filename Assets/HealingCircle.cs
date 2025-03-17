using System.Collections;
using UnityEngine;
using Photon.Pun;

public class HealingCircle : MonoBehaviour
{
    [Header("Healing Settings")]
    public int healAmount = 5;
    public float healInterval = 1f; // Heal every second

    [Header("Effect Settings")]
    [SerializeField] private GameObject healEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerHealth playerHealth))
        {
            StartCoroutine(HealOverTime(playerHealth));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerHealth playerHealth))
        {
            StopAllCoroutines();
        }
    }

    private IEnumerator HealOverTime(PlayerHealth playerHealth)
    {
        while (true)
        {
            playerHealth.Heal(healAmount);
            yield return new WaitForSeconds(healInterval);
        }
    }
}
