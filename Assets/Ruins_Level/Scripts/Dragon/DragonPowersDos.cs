using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DragonPowersDos : MonoBehaviour
{
    public GameObject flamethrowerPrefab;
    public GameObject flamePoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            Instantiate(flamethrowerPrefab, flamePoint.transform.position, flamePoint.transform.rotation);
        }
    }
}
