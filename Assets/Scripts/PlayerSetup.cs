using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    Camera playerCamera;
    // Start is called before the first frame update
    void Start()
    {
        playerCamera = Camera.main;
        if(photonView.IsMine)
        {
            GetComponent<CameraManager>().enabled = true;
            GetComponent<PlayerController>().enabled = true;
            GetComponent<PlayerHealth>().enabled = true;
            GetComponent<PlayerInput>().enabled = true;
            playerCamera.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
