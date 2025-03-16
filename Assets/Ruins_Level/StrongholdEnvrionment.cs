using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongholdEnvrionment : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + speed*Time.deltaTime, transform.eulerAngles.z);
    }
}
