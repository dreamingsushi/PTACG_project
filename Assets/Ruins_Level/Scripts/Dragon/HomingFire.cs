using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class HomingFire : MonoBehaviour
{
    public int currentFire;
    public DragonScaling dragonNumbers;

    [SerializeField] private float speed = 1.5f;
    public PlayerHealth[] players;
    [SerializeField] private float activeDuration = 15f;

    void Start()
    {
        AudioManager.Instance.PlaySFX("HomingFire");
        players = FindObjectsOfType<PlayerHealth>();
    }
    void OnEnable()
    {
        
        Invoke("DisappearAfterTime", activeDuration);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        Invoke("HomingFlames", 3);
    }

    private void OnTriggerEnter(Collider other) {
        
        if(other.gameObject.GetComponent<PlayerHealth>() != null)
        {
            int dragonDamage = (int)dragonNumbers.fireballDamage*2;
            other.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, dragonDamage, this.transform.position);

            //other.gameObject.GetComponent<PlayerHealth>().TakeDamage((int)dragonNumbers.fireballDamage, this.transform.position);
            

            
        }
        

        if(!other.gameObject.CompareTag("Dragon"))
            this.gameObject.SetActive(false);

        if(!other.gameObject.CompareTag("Flames"))
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
        GameObject player = players[currentFire].gameObject;
        float counter = speed*Time.deltaTime;
        this.transform.position = Vector3.Lerp(transform.position, player.transform.position, counter/activeDuration);

        
    }
}
