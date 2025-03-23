using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Flames : MonoBehaviour
{
    public int flameDamage;
    public DragonScaling dragonNumbers;
    private PlayerHealth playerEntered;

    private bool canFlameDamage;

    void OnEnable()
    {
        Invoke("DisappearAfterTime", 6.4f);
        flameDamage = (int)dragonNumbers.flamesDamage;
    }

    void Update()
    {
        if(canFlameDamage)
        {
            StartCoroutine(TakingFireDamage());
        }
    }

    private IEnumerator TakingFireDamage()
    {
        int dragonDamage = (int)dragonNumbers.flamesDamage; 
        playerEntered.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, dragonDamage, this.transform.position);
        canFlameDamage = false;
        yield return new WaitForSeconds(1.5f);
        canFlameDamage = true;
    }
    
    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerHealth>() != null)
        {
            playerEntered = other.gameObject.GetComponent<PlayerHealth>();
            canFlameDamage = true;

        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerHealth>() != null)
        {
            canFlameDamage = false;
        }
    }

    private void DisappearAfterTime()
    {
        Destroy(this.gameObject.transform.parent.gameObject);
    }
}
