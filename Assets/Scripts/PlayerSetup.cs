using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.UI;
using TMPro;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    
    public CinemachineVirtualCamera playerCamera;
    public Canvas hpUI;
    public Canvas WorldSpaceHP;
    public TMP_Text nameOfWarrior;
    public Canvas WorldSpaceName;
    // Start is called before the first frame update
    void Start()
    {
        
        if(photonView.IsMine)
        {
            GetComponent<CameraManager>().enabled = true;
            GetComponent<PlayerControllerPlus>().enabled = true;
            // GetComponent<PlayerHealth>().enabled = true;
            GetComponent<PlayerInput>().enabled = true;
            GetComponent<TimeCountDownManager>().enabled = true;
            hpUI.enabled = true;
            // imageHPUI.GetComponentInParent<Image>().enabled = true;
            // imageHPUI.SetActive(true);
            //GetComponentInChildren<HealthBar>().gameObject.SetActive(true);
            playerCamera.enabled = true;

            WorldSpaceHP.enabled = false;
            WorldSpaceName.enabled = false;
            
        }
        else
        {
            GetComponent<CameraManager>().enabled = false;
            GetComponent<PlayerControllerPlus>().enabled = false;
            // GetComponent<PlayerHealth>().enabled = false;
            GetComponent<PlayerInput>().enabled = false;
            GetComponent<TimeCountDownManager>().enabled = false;
            hpUI.enabled = false;
            // imageHPUI.GetComponentInParent<Image>().enabled = false;
            // imageHPUI.SetActive(false);
            //GetComponentInChildren<HealthBar>().gameObject.SetActive(false);
            playerCamera.enabled = false;

            WorldSpaceHP.enabled = true;
            WorldSpaceName.enabled = true;
            nameOfWarrior.text = GetComponent<PhotonView>().Owner.NickName;
        }
    }

}
