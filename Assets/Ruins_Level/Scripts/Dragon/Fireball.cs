using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    
    public float speed = 12.0f;
    public DragonPowers dragonPowers;

    void OnEnable()
    {
        dragonPowers = GameObject.FindObjectOfType<DragonPowers>();
        Invoke("DisappearAfterTime", 8);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position += -transform.forward * Time.deltaTime * speed;
    }

    private void OnTriggerEnter(Collider other) {
        
        if(other.gameObject.CompareTag("Power"))
        {
            Debug.Log("Hit");
            dragonPowers.dragonMeter ++;

            Destroy(other.gameObject);
        }
    }

    // void OnTriggerExit(Collider other)
    // {
    //     if(other.gameObject.GetComponent<BossController>())
    //     {
    //         Debug.Log("damaging");
    //     }
    // }

    private void DisappearAfterTime()
    {
        Destroy(this.gameObject);
    }
}
