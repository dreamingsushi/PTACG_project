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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            GameObject flaming = Instantiate(flamethrowerPrefab, flamePoint.transform.position, flamePoint.transform.rotation);
            flaming.transform.parent = this.transform;
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
