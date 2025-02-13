using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = this.transform.position - (this.transform.forward *8f* Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other) {
        Destroy(this);
    }
}
