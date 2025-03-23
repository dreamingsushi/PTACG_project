using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCollider : MonoBehaviour
{
    void Start()
    {
        Invoke("RemoveSmoke", 5f);
    }
    void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        BossHealth bossHealth = other.GetComponentInParent<BossHealth>();

        if (playerHealth != null)
        {
            playerHealth.ApplyHealingRegen();
        }

        if (bossHealth != null)
        {
            bossHealth.ApplyPoison();
        }
    }


    public void RemoveSmoke(){
        Destroy(gameObject);
    }
}
