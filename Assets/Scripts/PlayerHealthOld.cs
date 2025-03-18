using System.Collections;
using UnityEngine;
using System;
using Photon.Pun;

public class PlayerHealthOld : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

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

    [Header("Blood Effect Assign yi xia heheheheh :P ")]
    [SerializeField] private GameObject bloodEffect;
    private Coroutine regenCoroutine;
    private CharacterController characterController;
    private PlayerController playerController;

    [Header("HealthBar Assign tooooooo <3")]
    [SerializeField] private HealthBarSyncTest healthBar;


    void Start()
    {
        currentHealth = maxHealth;
        playerController = GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController>();
        //healthBar.SetHealth(currentHealth);

        if (canRegenerate)
            regenCoroutine = StartCoroutine(HealthRegeneration());
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            //TakeDamage(10, transform.position);
            GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 10, transform.position);
        }
    }

    [PunRPC]
    public void TakeDamage(int damage, Vector3 hitDirection)
    {
        if (isInvincible) return;

        int effectiveDamage = Mathf.Max(0, damage - armor);
        effectiveDamage = Mathf.RoundToInt(effectiveDamage * (1 - damageReductionPercent));

        currentHealth -= effectiveDamage;
        OnHealthChanged?.Invoke(currentHealth);

        ApplyKnockback(hitDirection);

        healthBar.SetHealth(currentHealth);
        
        StartCoroutine(DamageEffect());
        DamagePopUpText.Instance.ShowDamageNumber(transform.position, effectiveDamage.ToString());

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

        healthBar.SetHealth(currentHealth);
    }

    IEnumerator DamageEffect()
    {
        if(GetComponent<PhotonView>().IsMine)
        {
            bloodEffect.SetActive(true);
            yield return new WaitForSeconds(0.25f);
            bloodEffect.SetActive(false);
        }
        
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

    private IEnumerator Respawning(float respawnTime)
    {
        isInvincible = true;
        yield return new WaitForSeconds(respawnTime);
        isInvincible = false;
        playerController.isDead = false;
        GameStartManager.instance.currentDeaths -= 1;
        Heal(75);
    }

    private IEnumerator HealthRegeneration()
    {
        while (!playerController.isDead)
        {
            yield return new WaitForSeconds(regenInterval);
            Heal(regenAmount);
        }
    }

    private void Die()
    {
        Debug.Log("Player Died");
        GameStartManager.instance.currentDeaths += 1;
        playerController.isDead = true;
        OnPlayerDeath?.Invoke();
        StartCoroutine(Respawning(15f));
    }
}
