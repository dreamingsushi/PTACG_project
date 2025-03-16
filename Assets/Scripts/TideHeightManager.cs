using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class TideHeightManager : MonoBehaviour
{
    public float height;
    public float tideSpeed;
    
    public float highTide;
    public float lowTide;
    float originalHeight;
    
    void Start()
    {
        originalHeight = transform.position.y;
        height = originalHeight;
    }
    void Update()
    {
        Vector3 position = transform.position;
        position.y = height;
        transform.position = position;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TurnToLowTide();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ResetToOriginalHeight();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TurnToHighTide();
        }

    }
    public void ResetToOriginalHeight()
    {
        StopAllCoroutines();
        StartCoroutine(SmoothTransition(originalHeight));
    }
    public void TurnToHighTide()
    {
        StopAllCoroutines();
        StartCoroutine(SmoothTransition(highTide));
    }
    public void TurnToLowTide()
    {
        StopAllCoroutines();
        StartCoroutine(SmoothTransition(lowTide));
    }

    private IEnumerator SmoothTransition(float targetHeight)
    {
        while (Mathf.Abs(height - targetHeight) > 0.01f)
        {
            height = Mathf.Lerp(height, targetHeight, Time.deltaTime * tideSpeed);
            yield return null;
        }
        height = targetHeight;
    }
}
