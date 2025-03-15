using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;

public class BossSetup : MonoBehaviourPunCallbacks
{
    Camera PlayerCamera;
    // Start is called before the first frame update
    void Start()
    {
        PlayerCamera = Camera.main;
        if(photonView.IsMine)
        {
            GetComponentInChildren<BossMovement>().enabled = true;                 
            PlayerCamera.enabled = true;
            GetComponent<DragonPowers>().enabled = true;
            GetComponentInChildren<BossController>().enabled = true;
        }
    }

}
