using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DragonPowers : MonoBehaviour
{
    public int currentPhase;
    public GameObject fireball;
    public Transform _firePoint;
    public PlayableDirector cutscenePhase3;
    private Animator animator;
    
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F)){
            FireBallAttack();
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            ChangePhase();
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
               



        GameObject fireProjectile = Instantiate(fireball, _firePoint.position, _firePoint.rotation);
        //fireProjectile.transform.up = _hit.normal;
    }

    public void ChangePhase()
    {
        animator.SetTrigger("Phase");
        cutscenePhase3.Play();
        this.gameObject.GetComponent<BossMovement>().enabled = false;
    }
}
