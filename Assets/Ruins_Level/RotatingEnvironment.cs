using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingEnvironment : MonoBehaviour
{
    public float speed = 5f;

    private float multiplier = 1;

    // Start is called before the first frame update
    void Start()
    {
        multiplier = 1;
    }
    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, speed*multiplier*Time.deltaTime, 0, Space.Self);
        if(GameStartManager.instance.timeLeft < GameStartManager.instance.setTimeLimit/2)
        {
            multiplier = 2.2f;
        }
    }

    
}
