using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonPowers : MonoBehaviour
{
    public GameObject fireball;
    public Transform _firePoint;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F)){
            FireBallAttack();
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
}
