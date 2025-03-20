
using Photon.Pun;
using TMPro;
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
    public GameObject myCrossHair;
    private bool cursorLocked;
    private Vector3 relativeVector;

    void OnEnable()
    {
        if(GetComponent<PhotonView>().IsMine)
        {
            GetComponent<DragonPowersDos>().enabled = true;
            
            
            mySkills.SetActive(true);
            myCrossHair.SetActive(true);

        }
        else
        {
            GetComponent<DragonPowersDos>().enabled = false;
            
            mySkills.SetActive(false);
            myCrossHair.SetActive(false);

            this.enabled = false;
            

        }

    }

    private void Update() {
        mouseLook();

        transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x,focusPoint.transform.parent.eulerAngles.y,this.transform.rotation.eulerAngles.z);

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        myCrossHair.GetComponentInChildren<TMP_Text>().text = "Killed: " + GameStartManager.instance.currentDeaths.ToString();
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
        if(focusPoint.transform.eulerAngles.x > 75)
        {
            focusPoint.transform.eulerAngles = new Vector3(75,focusPoint.transform.eulerAngles.y,0);
        }

        if (focusPoint.transform.rotation.eulerAngles.x <10)
        {
            float clampedX = Mathf.Clamp(focusPoint.transform.rotation.eulerAngles.x, 10, 360);
            focusPoint.transform.rotation = Quaternion.Euler(clampedX, focusPoint.transform.rotation.eulerAngles.y, 0);
        }


        //horiz
        
        focusPoint.transform.parent.Rotate(transform.up*Input.GetAxis("Mouse X")*Time.deltaTime*sens);
        
        // if(focusPoint.transform.parent.eulerAngles.y > maxAngle)
        // {
        //     focusPoint.transform.parent.eulerAngles = new Vector3(0,maxAngle,0);
        // }
        // if(focusPoint.transform.parent.eulerAngles.y < minAngle)
        // {
        //     focusPoint.transform.parent.eulerAngles = new Vector3(0,minAngle,0);
        // }
        
        
    }

    


}
