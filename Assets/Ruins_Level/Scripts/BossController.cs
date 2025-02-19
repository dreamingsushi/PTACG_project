
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
        if(Input.GetKeyDown(KeyCode.L)) cursorLocked = cursorLocked ? false : true;
        if(!cursorLocked)return;

        Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;

        relativeVector = transform.InverseTransformPoint(focusPoint.transform.position);
        relativeVector /= relativeVector.magnitude;
        turnDirection = relativeVector.x / relativeVector.magnitude;
        

        //vert
        focusPoint.transform.eulerAngles = new Vector3(focusPoint.transform.eulerAngles.x + -Input.GetAxis("Mouse Y"), focusPoint.transform.eulerAngles.y,0);


        //horiz
        focusPoint.transform.parent.Rotate(transform.up*Input.GetAxis("Mouse X")*Time.deltaTime*100);

        
        
    }


}
