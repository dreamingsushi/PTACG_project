using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    public int damageAmount = 10;
    public bool canHitPlayer;

    private void ApplyDamage(Collider other)
    {
        BossHealth health = other.GetComponentInParent<BossHealth>();
        if (health != null)
        {
            Vector3 hitDirection = (other.transform.position - transform.position).normalized;
            health.TakeDamage((float)damageAmount);
        }
    }

    private void ApplyDamage(Collision collision)
    {
        BossHealth health = collision.gameObject.GetComponentInParent<BossHealth>();
        if (health != null)
        {
            Vector3 hitDirection = collision.contacts[0].normal;
            health.TakeDamage((float)damageAmount);
        }
    }

    private void ApplyDamageToPlayer(Collider collider)
    {
        PlayerHealth playerHealth = collider.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            Vector3 hitDirection = (collider.transform.position - transform.position).normalized;
            playerHealth.TakeDamage(damageAmount, hitDirection);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ApplyDamage(other);

        if (canHitPlayer)
        {
            ApplyDamageToPlayer(other);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        ApplyDamage(collision);

    }
}
