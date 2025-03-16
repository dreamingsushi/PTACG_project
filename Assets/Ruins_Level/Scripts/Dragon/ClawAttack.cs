using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawAttack : MonoBehaviour
{
    public DragonScaling dragonNumbers;
    private DragonPowers dragonPowers;
    // Start is called before the first frame update
    void Start()
    {
        dragonPowers = GameObject.FindObjectOfType<DragonPowers>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerHealth>() != null)
        {
            dragonPowers.dragonMeter++;
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage((int)dragonNumbers.clawDamage, this.transform.position);
        }
    }
}
