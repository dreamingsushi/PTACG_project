using System.Collections;
using UnityEngine;
using System;
using Photon.Pun;

public class PlayerHealth : MonoBehaviour
{
    public GameObject regenVFX;
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
    private PlayerControllerPlus playerController;

    [Header("HealthBar Assign tooooooo <3")]
    [SerializeField] private HealthBar healthBar;

    [Header("Poison Damage Settings")]
    public bool isPoisoned = false;
    public int healAmount = 1;
    public float healInterval = 1f;
    public float healDuration = 5f;

    [Header("World Canvas")]
    public Canvas worldCanvasHP;
    private bool damagePersisted = true;
    private float damagePersistenceInterval = 5.5f;

    

    void Start()
    {
        currentHealth = maxHealth;
        playerController = GetComponent<PlayerControllerPlus>();
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

        
        // if(damagePersisted == true)
        // {
            
            
        //     damagePersistenceInterval-=Time.unscaledDeltaTime;
        //     Debug.Log("Thos os count" + damagePersistenceInterval);
            
        //     if(damagePersistenceInterval <= 0f)
        //     {
        //         Debug.Log("Done lol");
        //         GetComponent<PhotonView>().RPC("SyncWorldSpaceHP", RpcTarget.AllBuffered, false);
        //         damagePersisted = false;
        //         damagePersistenceInterval = 5.5f;
        //     }
        // }
    }

    [PunRPC]
    public void TakeDamage(int damage, Vector3 hitDirection)
    {
        if (isInvincible) return;

        //GetComponent<PhotonView>().RPC("SyncWorldSpaceHP", RpcTarget.AllBuffered, true);
        int effectiveDamage = Mathf.Max(0, damage - armor);
        effectiveDamage = Mathf.RoundToInt(effectiveDamage * (1 - damageReductionPercent));

        currentHealth -= effectiveDamage;
        OnHealthChanged?.Invoke(currentHealth);

        ApplyKnockback(hitDirection);

        float currentPlayerHP = currentHealth;
        healthBar.GetComponent<PhotonView>().RPC("SyncAllHealthBarUI", RpcTarget.AllBuffered, currentPlayerHP);
        // healthBar.SetHealth(currentHealth);
        
        StartCoroutine(DamageEffect());
        DamagePopUpText.Instance.ShowDamageNumber(transform.position, effectiveDamage.ToString());

        if (currentHealth <= 0 && !playerController.isDead)
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

        float currentPlayerHP = currentHealth;
        healthBar.GetComponent<PhotonView>().RPC("SyncAllHealthBarUI", RpcTarget.AllBuffered, currentPlayerHP);
        //healthBar.SetHealth(currentHealth);
    }

    IEnumerator DamageEffect()
    {
        if(!GetComponent<PhotonView>().IsMine)
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
        GameStartManager.instance.GetComponent<PhotonView>().RPC("IncreasePlayerDeathCount", RpcTarget.AllBuffered, -1);
        isInvincible = false;
        playerController.isDead = false;
        
        Heal(75);
        AudioManager.Instance.PlaySFX("Respawn");
    }

    private IEnumerator HealthRegeneration()
    {
        while (!playerController.isDead)
        {
            yield return new WaitForSeconds(regenInterval);
            Heal(regenAmount);
        }
    }

    public void Die()
    {
        playerController.isDead = true;
        GameStartManager.instance.GetComponent<PhotonView>().RPC("IncreasePlayerDeathCount", RpcTarget.AllBuffered, 1);
        Debug.Log("Player Died");
        
        OnPlayerDeath?.Invoke();
        StartCoroutine(Respawning(15f));
    }

    private IEnumerator HealingEffect(float duration)
    {
        float elapsedTime = 0f;
        GetComponent<PhotonView>().RPC("SyncHealingEffect", RpcTarget.AllBuffered, true);
        while (elapsedTime < duration)
        {
            yield return new WaitForSeconds(healInterval);
            Heal(healAmount);
            elapsedTime += healInterval;
        }
        regenVFX.SetActive(false);
    }

    public void ApplyHealingRegen()
    {
        StartCoroutine(HealingEffect(healDuration));
    }

    [PunRPC]
    public void SyncHealingEffect(bool canHasEnabled)
    {
        regenVFX.SetActive(canHasEnabled);
    }

    [PunRPC]
    public void SyncWorldSpaceHP(bool isDamagedBefore)
    {
        damagePersisted = isDamagedBefore;
        if(!GetComponent<PhotonView>().IsMine)
            worldCanvasHP.enabled = isDamagedBefore;
    }
}
