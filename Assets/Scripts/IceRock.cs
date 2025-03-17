using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceRock : MonoBehaviour
{
    public GameObject iceSplashPrefab; // Assign this in the Inspector
    public float splashLifetime = 2f; // Time before the splash effect is destroyed
    public float splashOffsetY = 0.5f; // Adjust this value to control how low the splash appears

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Water")) 
        {
            if (iceSplashPrefab != null)
            {
                Vector3 splashPosition = transform.position - new Vector3(0, splashOffsetY, 0); // Lower the splash effect
                GameObject splashEffect = Instantiate(iceSplashPrefab, splashPosition, Quaternion.identity);
                Destroy(splashEffect, splashLifetime); // Destroy the splash effect after a delay
            }
            Destroy(gameObject);
        }
    }   
}
