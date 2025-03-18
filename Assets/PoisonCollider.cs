using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCollider : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        BossHealth bossHealth = other.GetComponent<BossHealth>();

        if (playerHealth != null)
        {
            playerHealth.ApplyHealingRegen();
        }

        if (bossHealth != null)
        {
            bossHealth.ApplyPoison();
        }
    }
}
