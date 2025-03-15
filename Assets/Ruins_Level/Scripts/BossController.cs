
using UnityEngine;

public class BossController : MonoBehaviour
{
    public GameObject focusPoint;
    public float turnDirection;

    private bool cursorLocked;
    private Vector3 relativeVector;
    

    private void Update() {
        mouseLook();
        
    }

    private void mouseLook()
    {
        // if(Input.GetKeyDown(KeyCode.L)) cursorLocked = cursorLocked ? false : true;
        // if(!cursorLocked)return;

        //Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;

        Cursor.lockState = CursorLockMode.Locked;
        relativeVector = transform.InverseTransformPoint(focusPoint.transform.position);
        relativeVector /= relativeVector.magnitude;
        turnDirection = relativeVector.x / relativeVector.magnitude;
        

        //vert
        focusPoint.transform.eulerAngles = new Vector3(focusPoint.transform.eulerAngles.x + -Input.GetAxis("Mouse Y"), focusPoint.transform.eulerAngles.y,0);

        //limit rotation
        float xRotation = focusPoint.transform.eulerAngles.x;

        if (xRotation > 180) 
        {
            // Convert from 360-degree system to a -180 to 180 range
            xRotation -= 360;
        }

        // Clamp the rotation between -80 and 80 degrees
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        focusPoint.transform.eulerAngles = new Vector3(xRotation, focusPoint.transform.eulerAngles.y, 0);

        //horiz
        focusPoint.transform.parent.Rotate(transform.up*Input.GetAxis("Mouse X")*Time.deltaTime*100);
        
        
        
    }


}
