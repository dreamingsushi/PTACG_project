using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public float maxBossHP;
    public float currentBossHP;
    public DragonScaling dragonNumbers;
    
    public event Action<float> OnHealthChanged;
    public CinemachineVirtualCamera deathCam;
    public GameObject changeSphere;
    public Material dragonDeathMat;
    private float armor = 10f;
    private bool dragonFade;
    [SerializeField] private HealthBar healthBar;

    void Awake()
    {
        dragonNumbers.takenDamage = 0f;
        if(GetComponentInChildren<BossController>() != null)
        {
            maxBossHP = 300f;
            currentBossHP = maxBossHP;
        }
        else if(GetComponent<BossControllerDos>() != null)
        {
            maxBossHP = 1000f;
            currentBossHP = 1000f - dragonNumbers.takenDamage;
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
    }

    public void TakeDamage(float damage)
    {
        
        float damageReductionPercent = armor*0.01f;
        float effectiveDamage = Mathf.Max(0, damage * armor);
        effectiveDamage = Mathf.RoundToInt(effectiveDamage * (1 - damageReductionPercent));

        currentBossHP -= effectiveDamage;
        dragonNumbers.takenDamage += effectiveDamage;
        OnHealthChanged?.Invoke(currentBossHP);

        

        healthBar.SetHealth(currentBossHP);
        
        if(GetComponentInChildren<BossController>() != null && GetComponentInChildren<PhotonView>().IsMine)
        {
            DamagePopUpText.Instance.ShowDamageNumber(GetComponentInChildren<BossController>().transform.position, damage.ToString());
        }
        else if(GetComponent<BossController>() != null && GetComponent<PhotonView>().IsMine)
        {
            DamagePopUpText.Instance.ShowDamageNumber(GetComponent<BossControllerDos>().transform.position, damage.ToString());
        }

        if (currentBossHP <= 0)
        {
            BossDie();
        }
    }

    public void BossDie()
    {
        Debug.Log("Boss is Dead");
        GameStartManager.instance.gameResulted = true;
        StartCoroutine(StartDying());
        
    }

    public IEnumerator StartDying()
    {
        yield return new WaitForSeconds(2);
        deathCam.enabled = true;
        deathCam.Priority = 50;
        if(GetComponentInChildren<BossController>() != null )
        {
            changeSphere.SetActive(true);
        }
        else if(GetComponent<BossControllerDos>() != null)
        {
            GetComponent<Animator>().SetBool("Death", true);
            yield return new WaitForSeconds(2);
            dragonFade = true;
            
        }

        yield return new WaitForSeconds(4.4f);
        if(GetComponent<PhotonView>().IsMine)
        {
            GameStartManager.instance.Defeat.SetActive(true);
        }
        else
            GameStartManager.instance.Victory.SetActive(true);

        yield return new WaitForSeconds(7f);
        Debug.Log("Go To Scene");

    }
}
