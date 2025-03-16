using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    public DragonScaling dragonNumber;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.parent.transform.eulerAngles = GameObject.FindObjectOfType<DragonPowersDos>().transform.eulerAngles;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerHealth>() != null)
        {
            other.GetComponent<PlayerHealth>().TakeDamage((int)dragonNumber.flamethrowerDamage, this.transform.position);
        }
    }
}
