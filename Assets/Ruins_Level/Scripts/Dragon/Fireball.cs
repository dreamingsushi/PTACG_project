using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    
    public float speed = 12.0f;
    public DragonPowers dragonPowers;
    public GameObject flamesPrefab;
    public DragonScaling dragonNumbers;
    private bool canFlames = true;

    void OnEnable()
    {
        //dragonPowers = GameObject.FindObjectOfType<DragonPowers>();

        Invoke("DisappearAfterTime", 8);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position += -transform.forward * Time.deltaTime * speed;
    }

    private void OnTriggerEnter(Collider other) {
        
        if(other.gameObject.GetComponent<PlayerHealth>() != null)
        {
            int dragonDamage = (int)dragonNumbers.fireballDamage;
            other.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, dragonDamage, this.transform.position);
            //other.gameObject.GetComponent<PlayerHealth>().TakeDamage((int)dragonNumbers.fireballDamage, this.transform.position);
            dragonPowers.dragonMeter ++;

            
        }
        

        // if(!other.gameObject.CompareTag("Dragon"))
        //     this.gameObject.SetActive(false);


        // if(!other.gameObject.CompareTag("Flames"))
        // {
        //     this.gameObject.SetActive(false);
        // }

        

        if(other.gameObject.layer == 6 && canFlames)
        {
            canFlames= false;
            GameObject flam = PhotonNetwork.Instantiate(flamesPrefab.name, this.transform.position, quaternion.identity);
            flam.GetComponentInChildren<Flames>().dragonNumbers = this.dragonNumbers;
            this.gameObject.SetActive(false);
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
