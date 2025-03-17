using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Photon.Pun;

public class DragonPowersDos : MonoBehaviour
{
    public GameObject flamethrowerPrefab;
    public GameObject flamePoint;
    public GameObject fireball;

    public GameObject[] fireballSpawnPoints = new GameObject[3];

    private float cd1 = 0.85f;
    private float cd2 = 13f;

    private bool canFlamethrower = true;
    private bool canSpawnHoming = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && canFlamethrower)
        {
            canFlamethrower = false;
            GameObject flaming = Instantiate(flamethrowerPrefab, flamePoint.transform.position, flamePoint.transform.rotation);
            flaming.transform.parent = this.transform;
        }

        if(Input.GetKeyDown(KeyCode.G) && canSpawnHoming)
        {
            canSpawnHoming = false;
            SpawnFireballsHoming();
        }

        if(canFlamethrower == false)
        {
            cd1 -= Time.deltaTime;
            if(cd1 < 0)
            {
                cd1 = 0.85f;
                canFlamethrower = true;
            }
        }

        if(canSpawnHoming == false)
        {
            cd2 -= Time.deltaTime;
            if(cd2 < 0)
            {
                cd2 = 13f;
                canSpawnHoming = true;
            }
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

    
}
