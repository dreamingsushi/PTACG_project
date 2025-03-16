using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;

public class DragonPowers : MonoBehaviour
{
    public int currentPhase;
    public GameObject fireball;
    public Transform _firePoint;
    public PlayableDirector cutscenePhase3;
    public GameObject focusPointCamera;


    public int dragonMeter;
    public GameObject evolveText;
    
    public int evolveRequirement = 2;
    private bool canEvolve;
    private Animator animator;
    private GameStartManager gameManager;
    
    
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
        if(Input.GetKeyDown(KeyCode.F)){
            FireBallAttack();
        }

        if(Input.GetMouseButtonDown(0))
        {
            ClawAttack();
        }

        if(Input.GetMouseButtonDown(1))
        {
            ClawAttack2();
        }

        if(Input.GetKeyDown(KeyCode.R) && canEvolve)
        {
            evolveText.SetActive(false);
            ChangePhase();
            
        }

        if(canEvolve)
        {
            evolveText.SetActive(true);
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

        PhotonNetwork.Instantiate(fireball.name, _firePoint.position, fireballAngle);
        // GameObject fireProjectile = Instantiate(fireball, _firePoint.position, fireballAngle);
        //fireProjectile.transform.up = _hit.normal;
    }

    public void ChangePhase()
    {
        gameManager.BossNextPhase();
        animator.SetTrigger("Phase");
        cutscenePhase3.Play();
        this.gameObject.GetComponentInChildren<BossMovement>().enabled = false;
    }

    public void ClawAttack()
    {
        animator.SetTrigger("swipe1");
    }

    public void ClawAttack2()
    {
        animator.SetTrigger("swipe2");
    }
}
