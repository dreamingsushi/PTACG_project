using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSlowEffect : MonoBehaviour
{
    public float upwardForce = 5f;
    public float playerLiftSpeed = 2f;
    
    private HashSet<Rigidbody> affectedBodies = new HashSet<Rigidbody>();
    private HashSet<CharacterController> affectedPlayers = new HashSet<CharacterController>();

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boss")) // Adjust tag if needed
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                affectedBodies.Add(rb);
            }
        }
        else if (other.CompareTag("Player")) // Adjust tag if needed
        {
            CharacterController cc = other.GetComponent<CharacterController>();
            if (cc != null)
            {
                affectedPlayers.Add(cc);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Boss"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                affectedBodies.Remove(rb);
            }
        }
        else if (other.CompareTag("Player"))
        {
            CharacterController cc = other.GetComponent<CharacterController>();
            if (cc != null)
            {
                affectedPlayers.Remove(cc);
            }
        }
    }

    void FixedUpdate()
    {
        foreach (Rigidbody rb in affectedBodies)
        {
            rb.AddForce(Vector3.up * upwardForce, ForceMode.Acceleration);
        }

        foreach (CharacterController cc in affectedPlayers)
        {
            cc.Move(Vector3.up * playerLiftSpeed * Time.deltaTime);
        }
    }
}
