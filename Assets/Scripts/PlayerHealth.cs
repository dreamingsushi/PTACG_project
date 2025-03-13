using System.Collections;
using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Armor Settings")]
    public int armor = 0; // Reduces damage taken
    public float damageReductionPercent = 0.1f; // 10% Damage Reduction

    [Header("Invincibility Settings")]
    public bool isInvincible = false;
    public float invincibilityDuration = 1f;

    [Header("Health Regeneration")]
    public bool canRegenerate = true;
    public int regenAmount = 1;
    public float regenInterval = 2f;

    [Header("Knockback Settings")]
    public float knockbackForce = 5f;
    
    public event Action<int> OnHealthChanged; // Event for UI updates
    public event Action OnPlayerDeath;

    [Header("Blood Effect Assign yi xia hehehehehehehheheh :P ")]
    [SerializeField] private GameObject bloodEffect;
    private Coroutine regenCoroutine;
    private CharacterController characterController;


    void Start()
    {
        currentHealth = maxHealth;
        characterController = GetComponent<CharacterController>();

        if (canRegenerate)
            regenCoroutine = StartCoroutine(HealthRegeneration());
        
    }

    public void TakeDamage(int damage, Vector3 hitDirection)
    {
        if (isInvincible) return;

        // Apply armor reduction
        int effectiveDamage = Mathf.Max(0, damage - armor);
        effectiveDamage = Mathf.RoundToInt(effectiveDamage * (1 - damageReductionPercent));

        currentHealth -= effectiveDamage;
        OnHealthChanged?.Invoke(currentHealth);

        // Apply Knockback
        ApplyKnockback(hitDirection);
        
        StartCoroutine(DamageEffect());
        DamagePopUpText.Instance.ShowDamageNumber(transform.position, damage.ToString());

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibilityFrames());
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        OnHealthChanged?.Invoke(currentHealth);
    }

    IEnumerator DamageEffect()
    {
        bloodEffect.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        bloodEffect.SetActive(false);
    }

    private void ApplyKnockback(Vector3 direction)
    {
        if (characterController != null)
        {
            Vector3 knockback = direction.normalized * knockbackForce;
            knockback.y = 0; // Prevent launching the player into the air
            characterController.Move(knockback * Time.deltaTime);
        }
    }

    private IEnumerator InvincibilityFrames()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }

    private IEnumerator HealthRegeneration()
    {
        while (true)
        {
            yield return new WaitForSeconds(regenInterval);
            Heal(regenAmount);
        }
    }

    private void Die()
    {
        Debug.Log("Player Died");
        OnPlayerDeath?.Invoke();
    }
}
