using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float powerMultiplier = 2;
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<BossMovement>())
        {
            other.gameObject.GetComponent<BossMovement>().JumpingWithForce(powerMultiplier);
        }
        
        if(other.gameObject.GetComponent<PlayerController>())
        {
            other.gameObject.GetComponent<PlayerController>().JumpWithForce(powerMultiplier);

        }
    }
}
