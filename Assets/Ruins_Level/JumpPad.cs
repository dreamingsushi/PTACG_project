using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<BossMovement>())
        {
            other.gameObject.GetComponent<BossMovement>().JumpingWithForce(1.8f);
        }
        
        if(other.gameObject.GetComponent<PlayerController>())
        {
            other.gameObject.GetComponent<PlayerController>().JumpWithForce(3.8f);

        }
    }
}
