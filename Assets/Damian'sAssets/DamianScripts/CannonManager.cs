using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonManager : MonoBehaviour
{
    public Animator[] Cannonset1;
    public Animator[] Cannonset2;
    public Animator[] Cannonset3;
    public float delaytime;

    void Start()
    {
        Invoke("Cannonattack1", delaytime);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Cannonattack1()
    {
        Debug.Log("aobin");
        foreach (Animator anim in Cannonset1)
        {
            
            anim.SetTrigger("Fall");
            
        }
        Invoke("Cannonattack2", delaytime);
    }

    void Cannonattack2()
    {
        foreach (Animator anim in Cannonset2)
        {

            anim.SetTrigger("Fall");
            
        }
        Invoke("Cannonattack3", delaytime);
    }

    void Cannonattack3()
    {
        foreach (Animator anim in Cannonset3)
        {

            anim.SetTrigger("Fall");
            
        }
        Invoke("Cannonattack1", delaytime);
    }

}
