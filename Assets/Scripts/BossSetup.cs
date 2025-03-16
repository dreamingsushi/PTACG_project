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
            GetComponentInChildren<BossMovement>().enabled = true;                 
            PlayerCamera.enabled = true;
            GetComponent<DragonPowers>().enabled = true;
            GetComponentInChildren<BossController>().enabled = true;
            GetComponentInChildren<HealthBar>().enabled = true;
        }
        else
        {
            GetComponentInChildren<BossMovement>().enabled = false;                 
            PlayerCamera.enabled = false;
            GetComponent<DragonPowers>().enabled = false;
            GetComponentInChildren<BossController>().enabled = false;
            GetComponentInChildren<HealthBar>().enabled = false;
        }
    }

}
