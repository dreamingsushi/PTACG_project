using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossHealth : MonoBehaviour
{
    public float maxBossHP;
    public float currentBossHP;
    public DragonScaling dragonNumbers;
    
    public event Action<float> OnHealthChanged;
    public CinemachineVirtualCamera deathCam;

    public bool isInvincible = false;
    public float invincibilityDuration = 0.75f;

    public GameObject deathVFX;
    public Material dragonDeathMat;
    private float armor = 10f;
    private bool dragonFade;
    [SerializeField] private HealthBar healthBar;

    [Header("Poison Damage Settings")]
    public bool isPoisoned = false;
    public int poisonDamage = 2;
    public float poisonInterval = 1f;
    public float poisonDuration = 5f;

    void Awake()
    {
        dragonNumbers.takenDamage = 0f;
        if(GetComponentInChildren<BossController>() != null)
        {
            maxBossHP = dragonNumbers.MaxHealthPoints1;
            currentBossHP = maxBossHP;
        }
        else if(GetComponent<BossControllerDos>() != null)
        {
            maxBossHP = dragonNumbers.MaxHealthPoints1;
            currentBossHP = dragonNumbers.MaxHealthPoints1 - dragonNumbers.takenDamage;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        dragonDeathMat.color = new Color(1,1,1,1);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(dragonFade)
        {
            Color dragonColor = dragonDeathMat.color;
            float fadeAmount = dragonColor.a - (0.2f*Time.deltaTime);
            
            dragonColor = new Color(dragonColor.r,dragonColor.g,dragonColor.b, fadeAmount);
            dragonDeathMat.color = dragonColor;
            Debug.Log(dragonColor.a);
            if(dragonColor.a <= 0)
            {
                dragonFade = false;
            }
        }

        if(!this.enabled)
        {
            StopAllCoroutines();
        }
        
    }

    [PunRPC]
    public void TakeDamage(float damage)
    {
        if(isInvincible) return;
        float damageReductionPercent = armor*0.01f;
        float effectiveDamage = Mathf.Max(0, damage);
        effectiveDamage = Mathf.RoundToInt(effectiveDamage * (1 - damageReductionPercent));

        currentBossHP -= effectiveDamage;
        
        OnHealthChanged?.Invoke(currentBossHP);

        
        
        healthBar.GetComponent<PhotonView>().RPC("SyncAllHealthBarUI", RpcTarget.AllBuffered, currentBossHP);
        //healthBar.SetHealth(currentBossHP);
        
        if(GetComponentInChildren<BossController>() != null)
        {
            //scriptable object
            GetComponentInChildren<BossDamageTaken>().photonView.RPC("SyncDamageTaken", RpcTarget.AllBuffered, effectiveDamage);
            DamagePopUpText.Instance.ShowDamageNumber(GetComponentInChildren<BossController>().gameObject.transform.position, effectiveDamage.ToString());
        }
        else if(GetComponent<BossController>() != null)
        {
            DamagePopUpText.Instance.ShowDamageNumber(GetComponent<BossControllerDos>().gameObject.transform.position, effectiveDamage.ToString());
        }

        if (currentBossHP <= 0)
        {
            GetComponent<PhotonView>().RPC("BossDie", RpcTarget.AllBuffered, true);
           
        }
        else
        {
            StartCoroutine(InvincibilityFrames());
        }

    }

    private IEnumerator InvincibilityFrames()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }

    [PunRPC]
    public void BossDie(bool canDieNow)
    {
        Debug.Log("Boss is Dead");
        GameStartManager.instance.gameResulted = canDieNow;
        StartCoroutine(StartDying());
        
    }

    public IEnumerator StartDying()
    {
        yield return new WaitForSeconds(2);
        deathCam.enabled = true;
        deathCam.Priority = 50;
        if(GetComponentInChildren<BossController>() != null )
        {
            deathVFX.SetActive(true);
            PhotonNetwork.Instantiate("winning_Explosion", GetComponentInChildren<BossController>().transform.position, Quaternion.identity);
        }
        else if(GetComponent<BossControllerDos>() != null)
        {
            GetComponent<Animator>().SetBool("Death", true);
            deathVFX.SetActive(true);
            yield return new WaitForSeconds(2);
            dragonFade = true;
            
        }

        yield return new WaitForSeconds(4.4f);
        if(GetComponentInChildren<PhotonView>().IsMine)
        {
            GameStartManager.instance.Defeat.SetActive(true);
        }
        else
            GameStartManager.instance.Victory.SetActive(true);

        yield return new WaitForSeconds(5f);
        
        SceneManager.LoadScene("WarriorVictory");

    }

    public void ApplyPoison()
    {
        if (!isPoisoned)
        {
            isPoisoned = true;
            StartCoroutine(PoisonEffect());
        }
    }

    private IEnumerator PoisonEffect()
    {
        float elapsedTime = 0f;

        while (elapsedTime < poisonDuration)
        {
            yield return new WaitForSeconds(poisonInterval);
            //TakeDamage(poisonDamage);
            float poison = (float)poisonDamage;
            GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, poison);
            elapsedTime += poisonInterval;
        }

        isPoisoned = false;
    }

}
