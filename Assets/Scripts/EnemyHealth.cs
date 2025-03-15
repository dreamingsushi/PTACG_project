using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;
    private Renderer bodyRenderer;
    [SerializeField] private Material hitMaterial;
    private Material originalMaterial;

    private void Start()
    {
        currentHealth = maxHealth;
        bodyRenderer = GetComponent<Renderer>();
        originalMaterial = bodyRenderer.material;
    }

    public void TakeDamage(int damage, Vector3 hitDirection)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage! Current Health: {currentHealth}");
        StartCoroutine(DamageEffect());
        DamagePopUpText.Instance.ShowDamageNumber(transform.position, damage.ToString());
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator DamageEffect()
    {
        bodyRenderer.material = hitMaterial;
        yield return new WaitForSeconds(0.2f);
        bodyRenderer.material = originalMaterial;
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        Destroy(gameObject); // Example death behavior
    }
}
