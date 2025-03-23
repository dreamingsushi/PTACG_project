using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;
using Unity.VisualScripting;

public class DragonPowers : MonoBehaviour
{
    public int currentPhase;
    public GameObject fireball;
    public GameObject flamesObject;
    public Transform _firePoint;
    public PlayableDirector cutscenePhase3;
    public GameObject focusPointCamera;


    public int dragonMeter;
    public GameObject evolveUI;
    
    public int evolveRequirement = 2;

    public Image fireballIcon;
    private bool canEvolve;
    private Animator animator;
    private GameStartManager gameManager;
    private bool canFireball = true;
    public float fireballCD = 0f;
    public DragonScaling dragonScales;
    private bool canClaw;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
        gameManager = GameStartManager.instance;
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(dragonMeter >= evolveRequirement)
        {
            canEvolve = true;
        }
        if(Input.GetKeyDown(KeyCode.F) && canFireball)
        {
            canFireball = false;
            FireBallAttack();
        }

        if(canFireball == false)
        {
            fireballCD += Time.deltaTime;
            fireballIcon.fillAmount = 1f;
            if(fireballCD > 1.1f)
            {
                fireballIcon.fillAmount = 1f - (fireballCD / 1.1f);
                fireballCD = 0;
                canFireball = true;
            }
        }

        

        if(Input.GetMouseButtonDown(0) && canClaw)
        {
            ClawAttack();
        }

        if(Input.GetMouseButtonDown(1) && canClaw)
        {
            ClawAttack2();
        }

        if(Input.GetKeyDown(KeyCode.R) && canEvolve)
        {
            
            ChangePhase();
            
        }

        if(canEvolve)
        {
            evolveUI.SetActive(true);
        }

        if(cutscenePhase3.state == PlayState.Playing)
        {
            Debug.Log("Done");
            this.gameObject.SetActive(false);
        }

        

        if(GetComponentInChildren<BossMovement>().isGrounded)
        {
            canClaw = true;
        }
        else
        {
            canClaw = false;
        }
        if(GameStartManager.instance.isSecondPhase)
        {
            StopAllCoroutines();
        }
    }

    public void FireBallAttack()
    {  
	    RaycastHit _hit; 
	    Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f));
 
	    if (Physics.Raycast(ray, out _hit, 100))
	    { 
                Debug.Log(_hit.collider.gameObject.name);
        } 
        
        Quaternion fireballAngle = Quaternion.Euler(-focusPointCamera.transform.rotation.eulerAngles.x,focusPointCamera.transform.parent.rotation.eulerAngles.y - 180, fireball.transform.rotation.eulerAngles.z);
        AudioManager.Instance.PlaySFX("FireBall");
        GameObject fireb = PhotonNetwork.Instantiate(fireball.name, _firePoint.position, fireballAngle);
        fireb.GetComponent<Fireball>().dragonNumbers = dragonScales;
        fireb.GetComponent<Fireball>().dragonPowers = this;
        fireb.GetComponent<Fireball>().flamesPrefab = flamesObject;
        // GameObject fireProjectile = Instantiate(fireball, _firePoint.position, fireballAngle);
        //fireProjectile.transform.up = _hit.normal;
        
    }

    public void ChangePhase()
    {
        cutscenePhase3.Play();
        gameManager.BossNextPhase();
        animator.SetTrigger("Phase");
        
        this.gameObject.GetComponentInChildren<BossMovement>().enabled = false;
        GameStartManager.instance.photonView.RPC("SyncBossPhase", RpcTarget.AllBuffered, true); 
        
    }

    public void ClawAttack()
    {
        animator.SetTrigger("swipe1");
    }

    public void ClawAttack2()
    {
        animator.SetTrigger("swipe2");
    }

    // private IEnumerator FireballCooldown()
    // {
    //     float elapsedTime = 0f;
    //     fireballIcon.fillAmount = 1f;

    //     while (elapsedTime < fireballCD)
    //     {
    //         elapsedTime += Time.deltaTime;
    //         fireballIcon.fillAmount = 1f - (elapsedTime / fireballCD);
    //         yield return null;
    //     }
    //     fireballIcon.fillAmount = 0f;
    //     canFireball = true;
    //     yield return null;
    // }
}
