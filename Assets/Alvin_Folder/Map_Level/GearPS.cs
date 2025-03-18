using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearPS : MonoBehaviour
{
    public ParticleSystem smokeEffect;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerPS()
    {
        if (smokeEffect != null)
        {
            smokeEffect.Play();
        }
    }
}
