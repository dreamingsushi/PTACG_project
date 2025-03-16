using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour
{
    [Header("Health Bar Components")]
    [SerializeField] private Image mainHealthFill;    // Red bar (actual HP)
    [SerializeField] private Image delayedHealthFill; // Yellow bar (delayed decay effect)

    [Header("Settings")]
    [SerializeField] private float smoothSpeed = 5f;  // Speed for red bar transition
    [SerializeField] private float delaySpeed = 2f;   // Speed for yellow bar transition
    [SerializeField] private float delayTime = 0.5f;  // Delay before yellow bar shrinks
    [SerializeField] private Gradient healthColorGradient; // Health color gradient

    private float maxHealth = 100f;
    private Coroutine delayedBarRoutine;

    void Start()
    {
        
    }
    public void SetMaxHealth(float maxHealthValue)
    {
        maxHealth = maxHealthValue;
        mainHealthFill.fillAmount = 1f;
        delayedHealthFill.fillAmount = 1f;
        UpdateHealthBarColor(1f);
    }

    public void SetHealth(float newHealth)
    {
        float targetFill = Mathf.Clamp01(newHealth / maxHealth);

        mainHealthFill.fillAmount = Mathf.Clamp01(targetFill);

        if (delayedBarRoutine != null)
            StopCoroutine(delayedBarRoutine);
        StartCoroutine(SmoothHealthUpdate(mainHealthFill, targetFill, smoothSpeed));

        delayedBarRoutine = StartCoroutine(DelayedHealthUpdate(targetFill));

        UpdateHealthBarColor(targetFill);
    }


    private IEnumerator SmoothHealthUpdate(Image bar, float targetFill, float speed)
    {
        float startFill = bar.fillAmount;
        float elapsedTime = 0f;

        while (!Mathf.Approximately(bar.fillAmount, targetFill))
        {
            elapsedTime += Time.deltaTime * speed;
            bar.fillAmount = Mathf.Lerp(startFill, targetFill, elapsedTime);
            yield return null;
        }

        bar.fillAmount = targetFill;
    }

    private IEnumerator DelayedHealthUpdate(float targetFill)
    {
        yield return new WaitForSeconds(delayTime); // Wait before yellow bar starts shrinking
        yield return SmoothHealthUpdate(delayedHealthFill, targetFill, delaySpeed);
    }

    private void UpdateHealthBarColor(float healthPercentage)
    {
        mainHealthFill.color = healthColorGradient.Evaluate(healthPercentage);
    }
}
