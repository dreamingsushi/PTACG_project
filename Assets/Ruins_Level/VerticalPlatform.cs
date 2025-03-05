using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalPlatform : MonoBehaviour
{
    public float mag = 0.5f;
    public float freq = 1f;
    Vector3 initPos;

    void Start()
    {
        mag = Random.Range(0.8f, 2.2f);
        freq = Random.Range(1f, 2.55f);
        initPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(initPos.x, Mathf.Sin(Time.time * freq) * mag + initPos.y, initPos.z);
    }
}
