
using Photon.Pun;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossControllerDos : MonoBehaviour
{
    public GameObject focusPoint;
    public float turnDirection;
    public float sens = 1f;
    public float maxAngle;
    public float minAngle;
    

    public GameObject mySkills;
    private bool cursorLocked;
    private Vector3 relativeVector;

    void OnEnable()
    {
        if(GetComponent<PhotonView>().IsMine)
        {
            GetComponent<DragonPowersDos>().enabled = true;
            
            
            mySkills.SetActive(true);

        }
        else
        {
            GetComponent<DragonPowersDos>().enabled = false;
            
            mySkills.SetActive(false);
            this.enabled = false;
            

        }

    }

    private void Update() {
        mouseLook();

        transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x,focusPoint.transform.parent.eulerAngles.y,this.transform.rotation.eulerAngles.z);
    }

    private void mouseLook()
    {
        // if(Input.GetKeyDown(KeyCode.J)) cursorLocked = cursorLocked ? false : true;
        // if(!cursorLocked)return;

        Cursor.lockState = CursorLockMode.Locked;

        relativeVector = transform.InverseTransformPoint(focusPoint.transform.position);
        relativeVector /= relativeVector.magnitude;
        turnDirection = relativeVector.x / relativeVector.magnitude;
        

        //vert
        focusPoint.transform.eulerAngles = new Vector3(focusPoint.transform.eulerAngles.x + -Input.GetAxis("Mouse Y"), focusPoint.transform.eulerAngles.y,0);
        if(focusPoint.transform.eulerAngles.x > 35)
        {
            focusPoint.transform.eulerAngles = new Vector3(35,focusPoint.transform.eulerAngles.y,0);
        }

        if(focusPoint.transform.eulerAngles.x < 1)
        {
            focusPoint.transform.eulerAngles = new Vector3(1,focusPoint.transform.eulerAngles.y,0);
        }


        //horiz
        
        focusPoint.transform.parent.Rotate(transform.up*Input.GetAxis("Mouse X")*Time.deltaTime*sens);
        
        if(focusPoint.transform.parent.eulerAngles.y > maxAngle)
        {
            focusPoint.transform.parent.eulerAngles = new Vector3(0,maxAngle,0);
        }
        if(focusPoint.transform.parent.eulerAngles.y < minAngle)
        {
            focusPoint.transform.parent.eulerAngles = new Vector3(0,minAngle,0);
        }
        
        
    }

    


}
