using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;

public class BossDamageTaken : MonoBehaviourPunCallbacks
{
    public DragonScaling dragonNo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    public void SyncDamageTaken(float dmg)
    {
        dragonNo.takenDamage += dmg;
    }
}
