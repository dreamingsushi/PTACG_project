using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class TimeCountDownManager : MonoBehaviourPun
{
    private TMP_Text timeUIText;
    private float timeToStartRace = 2.5f;

    private void Awake()
    {
        timeUIText = GameStartManager.instance.TimeUIText;
    }
 
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<PlayerController>().enabled = false;
        timeUIText.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (timeToStartRace >= 0.0f)
            {
                timeToStartRace -= Time.deltaTime;
                int timerInt = (int)timeToStartRace;
                photonView.RPC("SetTime", RpcTarget.AllBuffered, timerInt);
            }
            else if (timeToStartRace < 0.0f)
            {
                photonView.RPC("StartTheGame", RpcTarget.AllBuffered);
            }
        }
    }
 
    [PunRPC]
    public void SetTime(int time)
    {
        if (time > 0.0f)
        {
            timeUIText.text = "Defeat the Boss";
        }
        else
        {
            //The countdown is over
            timeUIText.text = "";
        }
    }

    [PunRPC]
    public void StartTheGame()
    {
        GetComponent<PlayerController>().enabled = true;
        this.enabled = false;
    }
}
