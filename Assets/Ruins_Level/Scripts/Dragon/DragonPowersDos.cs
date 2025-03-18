using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class DragonPowersDos : MonoBehaviour
{
    public GameObject flamethrower;
    public GameObject flamePoint;
    public GameObject fireball;

    public GameObject[] fireballSpawnPoints = new GameObject[3];

    private float cd1 = 1.25f;
    private float cd2 = 12f;

    public Image flamethrowerIcon;
    public Image homingIcon;

    public bool canFlamethrower = true;
    public bool canSpawnHoming = true;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && canFlamethrower)
        {        
            GetComponent<PhotonView>().RPC("EnableFlamethrower", RpcTarget.AllBuffered, true); 
            
        }

        if(Input.GetKeyDown(KeyCode.G) && canSpawnHoming)
        {
            canSpawnHoming = false;
            SpawnFireballsHoming();
        }

        if(canFlamethrower == false)
        {
            StartCoroutine(FlamethrowerCooldown());
        }

        if(canSpawnHoming == false)
        {
            StartCoroutine(HomingCooldown());
        }

        
    }

    public void SpawnFireballsHoming()
    {
        int i = 0;
        foreach(GameObject obj in fireballSpawnPoints)
        {
            
            GameObject fireballNew = PhotonNetwork.Instantiate(fireball.name, obj.transform.position, obj.transform.rotation);
            fireballNew.GetComponent<HomingFire>().currentFire = i;
            i++;
        }
        
    }

    private IEnumerator FlamethrowerCooldown()
    {
        float elapsedTime = 0f;
        flamethrowerIcon.fillAmount = 1f;

        while (elapsedTime < cd1)
        {
            elapsedTime += Time.deltaTime;
            flamethrowerIcon.fillAmount = 1f - (elapsedTime / cd1);
            yield return null;
        }
        flamethrowerIcon.fillAmount = 0f;
        canFlamethrower = true;
    }

    private IEnumerator HomingCooldown()
    {
        float elapsedTime = 0f;
        homingIcon.fillAmount = 1f;

        while (elapsedTime < cd2)
        {
            elapsedTime += Time.deltaTime;
            homingIcon.fillAmount = 1f - (elapsedTime / cd2);
            yield return null;
        }
        homingIcon.fillAmount = 0f;
        canSpawnHoming = true;
    }

    [PunRPC]
    public void EnableFlamethrower(bool canFlameNow)
    {
        flamethrower.SetActive(canFlameNow);
    }

    
}
