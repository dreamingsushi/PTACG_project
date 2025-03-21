using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStoneController : MonoBehaviour
{
    public Animator[] animators;
    public Animator[] Delayanimators;
    public float delaytime;

    void Start()
    {
        if (animators.Length == 0)
        {
            Debug.LogError("No animators assigned!");
        }
        Invoke("FSTShoot", 20f);
        
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    foreach (Animator anim in animators)
        //    {
        //        anim.SetTrigger("Start");
        //        Invoke("FSEAnimation", delaytime);
        //    }
        //}
    }

    void FSTShoot()
    {
        foreach (Animator anim in animators)
        {
            anim.SetTrigger("Start");
            
        }
        Invoke("FSEAnimation", delaytime);
    }

    void FSEAnimation()
    {
        foreach (Animator anim in Delayanimators)
        {
            anim.SetTrigger("up");
        }
        Invoke("FSTShoot", 50f);
    }

    //void FSEDAnimation()
    //{
    //    foreach (Animator anim in Delayanimators)
    //    {
    //        anim.SetTrigger("down");
    //    }
    //}

}
