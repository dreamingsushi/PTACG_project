using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEstingRuins : MonoBehaviour
{
    public Material blue;
    public Material red;
    public Material purple;
    public GameObject crystal;
    float counter =0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if(Input.GetKeyDown(KeyCode.Alpha1))
        // {
        //     crystal.GetComponent<MeshRenderer>().material = blue;
        // }
        // else if(Input.GetKeyDown(KeyCode.Alpha2))
        // {
        //     crystal.GetComponent<MeshRenderer>().material = red;
        // }
        // else if(Input.GetKeyDown(KeyCode.Alpha3))
        // {
        //     crystal.GetComponent<MeshRenderer>().material = purple;
        // }
        counter += 1f*Time.deltaTime;
        transform.position = Vector3.Lerp(transform.position, crystal.transform.position, counter/15);
    }
}
