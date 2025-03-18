using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using Cinemachine;
using UnityEngine.UI;


public class BossSetup : MonoBehaviourPunCallbacks
{
    public CinemachineVirtualCamera PlayerCamera;
    public Canvas HPCanvas;

    // Start is called before the first frame update
    void Start()
    {
        
        if(photonView.IsMine)
        {
            GetComponent<BossMovement>().enabled = true;                 
            PlayerCamera.enabled = true;
            GetComponentInParent<DragonPowers>().enabled = true;
            GetComponent<BossController>().enabled = true;
            HPCanvas.enabled = true;
            // imageHPUI.GetComponentInParent<Image>().enabled = true;
            // imageHPUI.SetActive(true);
            //transform.parent.gameObject.GetComponentInChildren<HealthBar>().gameObject.SetActive(true);
        }
        else
        {
            GetComponent<BossMovement>().enabled = false;                 
            PlayerCamera.enabled = false;
            GetComponentInParent<DragonPowers>().enabled = false;
            GetComponent<BossController>().enabled = false;
            HPCanvas.enabled = false;
            // imageHPUI.GetComponentInParent<Image>().enabled = false;
            // imageHPUI.SetActive(false);
            //transform.parent.gameObject.GetComponentInChildren<HealthBar>().gameObject.SetActive(false);
        }
    }

}
