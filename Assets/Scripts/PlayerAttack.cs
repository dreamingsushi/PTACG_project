using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject ballPrefab;  // Assign the ball prefab in Inspector
    public Transform shootPoint;   // Assign a point (like a gun or hand position)
    public float ballSpeed = 20f;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse click
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Camera cam = Camera.main;
        Ray ray = cam.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point; // Hit something, use its position
        }
        else
        {
            targetPoint = ray.origin + ray.direction * 100f; // Shoot far if no hit
        }

        // Instantiate and shoot the ball
        GameObject ball = Instantiate(ballPrefab, shootPoint.position, Quaternion.identity);
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        Vector3 shootDirection = (targetPoint - shootPoint.position).normalized;
        rb.velocity = shootDirection * ballSpeed;
    }
}
