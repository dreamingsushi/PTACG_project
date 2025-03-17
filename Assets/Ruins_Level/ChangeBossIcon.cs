using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ChangeBossIcon : MonoBehaviour
{
    public Sprite dragonPhase2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameStartManager.instance.isSecondPhase)
        {
            this.GetComponent<Image>().sprite = dragonPhase2;
        }
    }
}
