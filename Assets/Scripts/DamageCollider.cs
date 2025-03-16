using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    public int damageAmount = 10;
    public bool canHitPlayer;

    private void ApplyDamage(Collider other)
    {
        EnemyHealth health = other.GetComponent<EnemyHealth>();
        if (health != null)
        {
            Vector3 hitDirection = (other.transform.position - transform.position).normalized;
            health.TakeDamage(damageAmount, hitDirection);
        }
    }

    private void ApplyDamage(Collision collision)
    {
        EnemyHealth health = collision.gameObject.GetComponent<EnemyHealth>();
        if (health != null)
        {
            Vector3 hitDirection = collision.contacts[0].normal;
            health.TakeDamage(damageAmount, hitDirection);
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
