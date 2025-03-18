using JetBrains.Annotations;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class treechopManager : MonoBehaviour
{
    Animator treeAnimator;
    public bool isChopped1 = false;
    public float GrowTime = 5f;

    void Start()
    {
        treeAnimator = GetComponent<Animator>();
        

    }


    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("aobin");

        if (collision.gameObject.tag == "Dragon" && isChopped1 == false)
        {
            treeAnimator.SetTrigger("Chopped1");
            isChopped1 = true;

        }
        else if (collision.gameObject.tag == "Dragon" && isChopped1 == true)
        {
            treeAnimator.SetTrigger("Chopped2");
            isChopped1 = false;
            gameObject.GetComponent<Collider>().enabled = false;
            Invoke("Grow", GrowTime);

        }

    }

    void Grow()
    {
        treeAnimator.SetTrigger("Grow");
        gameObject.GetComponent<Collider>().enabled = true;
    }


}
