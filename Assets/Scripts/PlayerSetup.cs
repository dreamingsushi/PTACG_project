using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.UI;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    
    public CinemachineVirtualCamera playerCamera;
    public GameObject imageHPUI;
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
            imageHPUI.GetComponentInParent<Image>().enabled = true;
            imageHPUI.SetActive(true);
            //GetComponentInChildren<HealthBar>().gameObject.SetActive(true);
            playerCamera.enabled = true;
        }
        else
        {
            GetComponent<CameraManager>().enabled = false;
            GetComponent<PlayerController>().enabled = false;
            GetComponent<PlayerHealth>().enabled = false;
            GetComponent<PlayerInput>().enabled = false;
            GetComponent<TimeCountDownManager>().enabled = false;
            imageHPUI.GetComponentInParent<Image>().enabled = false;
            imageHPUI.SetActive(false);
            //GetComponentInChildren<HealthBar>().gameObject.SetActive(false);
            playerCamera.enabled = false;
        }
    }

}
