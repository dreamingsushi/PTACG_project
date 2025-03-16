using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public float maxBossHP;
    public float currentBossHP;
    public DragonScaling dragonNumbers;
    private float armor = 10f;
    public event Action<float> OnHealthChanged;
    
    [SerializeField] private HealthBar healthBar;

    void Awake()
    {
        if(GetComponent<BossController>() != null)
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        
        float damageReductionPercent = armor*0.01f;
        float effectiveDamage = Mathf.Max(0, damage * armor);
        effectiveDamage = Mathf.RoundToInt(effectiveDamage * (1 - damageReductionPercent));

        currentBossHP -= effectiveDamage;
        OnHealthChanged?.Invoke(currentBossHP);

        

        healthBar.SetHealth(currentBossHP);
        
        
        DamagePopUpText.Instance.ShowDamageNumber(transform.position, damage.ToString());

        if (currentBossHP <= 0)
        {
            BossDie();
        }
    }

    public void BossDie()
    {
        Debug.Log("Boss is Dead");
    }
}
