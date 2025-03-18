using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    public Material purple;

    public Material red;    
    public Material blue;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().material = blue;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 5f*Time.deltaTime, transform.eulerAngles.z);

        if(GameStartManager.instance.isSecondPhase)
        {
            this.GetComponent<MeshRenderer>().material = purple;
        }
        else if(GameStartManager.instance.timeLeft < GameStartManager.instance.setTimeLimit/2)
        {
            this.GetComponent<MeshRenderer>().material = purple;
        }
        else if(GameStartManager.instance.timeLeft < GameStartManager.instance.setTimeLimit*(1/4))
        {
            this.GetComponent<MeshRenderer>().material = red;
        }

        
    }
}
