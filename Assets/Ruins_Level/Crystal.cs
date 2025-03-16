using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 5f*Time.deltaTime, transform.eulerAngles.z);
    }
}
