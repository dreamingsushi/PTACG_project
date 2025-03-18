using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ChangeBossIcon : MonoBehaviour
{
    public GameObject dragonPhase2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameStartManager.instance.isSecondPhase)
        {
            dragonPhase2.SetActive(false);
        }
    }
}
