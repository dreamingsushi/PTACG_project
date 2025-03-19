using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
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
            float theDamageAmount = damageAmount;
            health.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, theDamageAmount);
        }
    }

    private void ApplyDamage(Collision collision)
    {
        BossHealth health = collision.gameObject.GetComponentInParent<BossHealth>();
        if (health != null)
        {
            Vector3 hitDirection = collision.contacts[0].normal;
            float theDamageAmount = damageAmount;
            health.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, theDamageAmount);
            //health.TakeDamage((float)damageAmount);

            // health.GetComponentInChildren<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, (float)damageAmount);
        }
    }

    private void ApplyDamageToPlayer(Collider collider)
    {
        PlayerHealth playerHealth = collider.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            Vector3 hitDirection = (collider.transform.position - transform.position).normalized;
            playerHealth.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, damageAmount, hitDirection);
            // playerHealth.TakeDamage(damageAmount, hitDirection);
        }
    }

    private void ApplyDamageToPlayer2(Collision collision)
    {
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            Vector3 hitDirection = collision.contacts[0].normal;
            playerHealth.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, damageAmount, hitDirection);
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
        if (canHitPlayer)
        {
            ApplyDamageToPlayer2(collision);
        }
    }
}
