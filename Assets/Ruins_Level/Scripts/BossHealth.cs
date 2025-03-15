using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public int maxBossHP;
    public int currentBossHP;

    void Awake()
    {
        if(GetComponent<BossController>() != null)
        {
            maxBossHP = 1000;
        }
        else if(GetComponent<BossControllerDos>() != null)
        {
            maxBossHP = 10000;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
