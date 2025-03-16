using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingFire : MonoBehaviour
{
    public int currentFire;
    public DragonScaling dragonNumbers;

    [SerializeField] private float speed;
    public object[] players;

    void Start()
    {
        
        players = FindObjectsOfType<PlayerHealth>();
    }
    void OnEnable()
    {
        
        Invoke("DisappearAfterTime", 16);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Invoke("HomingFlames", 3);
    }

    private void OnTriggerEnter(Collider other) {
        
        if(other.gameObject.GetComponent<PlayerHealth>() != null)
        {
            other.gameObject.GetComponent<PlayerHealth>().TakeDamage((int)dragonNumbers.fireballDamage, this.transform.position);
            

            
        }
        

        if(!other.gameObject.CompareTag("Dragon"))
            this.gameObject.SetActive(false);

        
    }

    // void OnTriggerExit(Collider other)
    // {
    //     if(other.gameObject.GetComponent<BossController>())
    //     {
    //         Debug.Log("damaging");
    //     }
    // }

    private void DisappearAfterTime()
    {
        Destroy(this.gameObject);
    }

    private void HomingFlames()
    {
        GameObject player = (GameObject) players[currentFire];
        this.transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.time);

        
    }
}
