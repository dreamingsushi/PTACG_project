using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    
    public CinemachineVirtualCamera playerCamera;
    // Start is called before the first frame update
    void Start()
    {
        
        if(photonView.IsMine)
        {
            GetComponent<CameraManager>().enabled = true;
            GetComponent<PlayerController>().enabled = true;
            GetComponent<PlayerHealth>().enabled = true;
            GetComponent<PlayerInput>().enabled = true;
            GetComponent<TimeCountDownManager>().enabled = true;
            GetComponentInChildren<HealthBar>().enabled = true;
            playerCamera.enabled = true;
        }
        else
        {
            GetComponent<CameraManager>().enabled = false;
            GetComponent<PlayerController>().enabled = false;
            GetComponent<PlayerHealth>().enabled = false;
            GetComponent<PlayerInput>().enabled = false;
            GetComponent<TimeCountDownManager>().enabled = false;
            GetComponentInChildren<HealthBar>().enabled = false;
            playerCamera.enabled = false;
        }
    }

}
