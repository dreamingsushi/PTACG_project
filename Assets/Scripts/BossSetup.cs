using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using Cinemachine;

public class BossSetup : MonoBehaviourPunCallbacks
{
    public CinemachineVirtualCamera PlayerCamera;
    // Start is called before the first frame update
    void Start()
    {
        
        if(photonView.IsMine)
        {
            GetComponent<BossMovement>().enabled = true;                 
            PlayerCamera.enabled = true;
            GetComponentInParent<DragonPowers>().enabled = true;
            GetComponent<BossController>().enabled = true;
            transform.parent.gameObject.GetComponentInChildren<HealthBar>().enabled = true;
        }
        else
        {
            GetComponent<BossMovement>().enabled = false;                 
            PlayerCamera.enabled = false;
            GetComponentInParent<DragonPowers>().enabled = false;
            GetComponent<BossController>().enabled = false;
            transform.parent.gameObject.GetComponentInChildren<HealthBar>().enabled = false;
        }
    }

}
