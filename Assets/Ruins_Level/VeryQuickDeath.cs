using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class VeryQuickDeath : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.GetComponentInParent<PlayerMovement>() != null)
        {
            EditorApplication.isPlaying = false;
        }
    }
}
