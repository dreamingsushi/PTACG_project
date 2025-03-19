using UnityEngine;
using System.Collections;

public class TideHeightManager : MonoBehaviour
{
    public float height;
    public float tideSpeed;

    public float highTide;
    public float lowTide;
    private float originalHeight;

    public float tideCycleDuration = 60f; // Full cycle duration (low → high → low)
    private bool hasTriggeredHighTide = false;

    private void Start()
    {
        originalHeight = transform.position.y;
        height = originalHeight;
        
        StartCoroutine(TideCycle()); // Start automatic tide cycle
    }

    private void Update()
    {
        // Update position based on height
        Vector3 position = transform.position;
        position.y = height;
        transform.position = position;

        // Manually override with key inputs
        if (Input.GetKeyDown(KeyCode.Alpha1)) TurnToLowTide();
        if (Input.GetKeyDown(KeyCode.Alpha2)) ResetToOriginalHeight();
        if (Input.GetKeyDown(KeyCode.Alpha3)) TurnToHighTide();

        // Check game time and force high tide when timeLeft <= 300s
        if (GameStartManager.instance.timeLeft <= 300f && !hasTriggeredHighTide)
        {
            hasTriggeredHighTide = true;
            StopAllCoroutines();
            StartCoroutine(SmoothTransition(highTide));
        }
    }

    private IEnumerator TideCycle()
    {
        while (true)
        {
            if (hasTriggeredHighTide) yield break; // Stop cycling if forced high tide is triggered

            yield return StartCoroutine(SmoothTransition(lowTide));  // Go to low tide
            yield return new WaitForSeconds(tideCycleDuration / 2);  // Stay at low tide

            yield return StartCoroutine(SmoothTransition(highTide)); // Go to high tide
            yield return new WaitForSeconds(tideCycleDuration / 2);  // Stay at high tide
        }
    }

    public void ResetToOriginalHeight()
    {
        StopAllCoroutines();
        hasTriggeredHighTide = false;
        StartCoroutine(SmoothTransition(originalHeight));
    }

    public void TurnToHighTide()
    {
        StopAllCoroutines();
        hasTriggeredHighTide = false;
        StartCoroutine(SmoothTransition(highTide));
    }

    public void TurnToLowTide()
    {
        StopAllCoroutines();
        hasTriggeredHighTide = false;
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
