using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    public DragonScaling dragonNumber;
    public GameObject focusPointCamera;

    public GameObject headPosition;
    // Start is called before the first frame update
    void OnEnable()
    {
        Invoke("DisappearAfterTime", 8);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = headPosition.transform.position;
        Quaternion flameAngle = Quaternion.Euler(focusPointCamera.transform.rotation.eulerAngles.x,this.transform.eulerAngles.y, this.transform.rotation.eulerAngles.z);
        this.transform.rotation = flameAngle;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerHealth>() != null)
        {
            int dragonDamage = (int)dragonNumber.flamethrowerDamage;
            other.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, dragonDamage, this.transform.position);
            //other.GetComponent<PlayerHealth>().TakeDamage((int)dragonNumber.flamethrowerDamage, this.transform.position);
        }
    }

    private void DisappearAfterTime()
    {
        GetComponentInParent<DragonPowersDos>().canFlamethrower = false;
        this.gameObject.SetActive(false);
    }
}
