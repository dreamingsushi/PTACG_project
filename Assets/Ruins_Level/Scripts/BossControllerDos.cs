
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class BossControllerDos : MonoBehaviour
{
    public GameObject focusPoint;
    public float turnDirection;
    public float sens = 10f;
    public float maxAngle;
    public float minAngle;

    private bool cursorLocked;
    private Vector3 relativeVector;
    

    private void Update() {
        mouseLook();

        transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles.x,focusPoint.transform.parent.eulerAngles.y,this.transform.rotation.eulerAngles.z);
    }

    private void mouseLook()
    {
        if(Input.GetKeyDown(KeyCode.J)) cursorLocked = cursorLocked ? false : true;
        if(!cursorLocked)return;

        Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;

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
