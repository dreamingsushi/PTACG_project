using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOManager : MonoBehaviour
{
    public Animator[] UFOset1;
    public Animator[] UFOset2;
    public Animator[] UFOset3;
    public Animator[] UFOset4;
    public float delaytime;

    void Start()
    {
        Invoke("UFOattack1", delaytime);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UFOattack1()
    {
        foreach (Animator anim in UFOset1)
        {
            
            anim.SetTrigger("Attack");
            
        }
        Invoke("UFOattack2", delaytime);
    }

    void UFOattack2()
    {
        foreach (Animator anim in UFOset2)
        {
            
            anim.SetTrigger("Attack");
            
        }
        Invoke("UFOattack3", delaytime);
    }

    void UFOattack3()
    {
        foreach (Animator anim in UFOset3)
        {
            
            anim.SetTrigger("Attack");
            
        }
        Invoke("UFOattack4", delaytime);
    }

    void UFOattack4()
    {
        foreach (Animator anim in UFOset4)
        {
            
            anim.SetTrigger("Attack");
            
        }
        Invoke("UFOattack1", delaytime);
    }
}
