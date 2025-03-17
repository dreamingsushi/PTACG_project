using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene1Phase2 : MonoBehaviour
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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (Animator anim in animators)
            {
                anim.SetBool("Phase2", true);
                anim.SetTrigger("Phase2");
                //Invoke("FSEAnimation", delaytime);
            }
        }
    }
}
