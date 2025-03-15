using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingEnvironment : MonoBehaviour
{
    public float speed = 5f;
    
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, speed*Time.deltaTime, 0, Space.Self);
    }

    
}
