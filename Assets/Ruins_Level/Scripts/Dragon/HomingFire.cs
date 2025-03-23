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

    private float count;

    float counter;
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
    void Update()
    {
        count += Time.deltaTime;
        Invoke("HomingFlames", 3);
    }

    private void OnTriggerEnter(Collider other) {
        
        if(other.gameObject.GetComponent<PlayerHealth>() != null)
        {
            int dragonDamage = (int)dragonNumbers.fireballDamage*2;
            other.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, dragonDamage, this.transform.position);

            //other.gameObject.GetComponent<PlayerHealth>().TakeDamage((int)dragonNumbers.fireballDamage, this.transform.position);
            

            
        }
        

        // if(!other.gameObject.CompareTag("Dragon"))
        //     this.gameObject.SetActive(false);

        // if(!other.gameObject.CompareTag("Flames"))
        //     this.gameObject.SetActive(false);
        
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
        if(players.Length < 3)
        {
            this.transform.eulerAngles = new Vector3(0, count*speed*6f, 0);
            
        }
        else
        {
            GameObject player = players[currentFire].gameObject;
            counter = speed*Time.deltaTime;
            this.transform.position = Vector3.Lerp(transform.position, player.transform.position, counter/activeDuration);
        }
        

        
    }
}
