using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class TimeCountDownManager : MonoBehaviourPun
{
    private TMP_Text timeUIText;
    private float timeToStartRace = 3.5f;
    private bool timerDone;
    

    private void Awake()
    {
        timeUIText = GameStartManager.instance.TimeUIText;
    }
 
    // Start is called before the first frame update
    void Start()
    {
        
        if(GetComponent<PlayerSetup>() != null && photonView.IsMine)
        {
            GetComponent<PlayerControllerPlus>().enabled = false;
        }

        if(GetComponent<BossSetup>() != null && photonView.IsMine)
        {
            GetComponent<BossController>().enabled = false;
        }
        
        timeUIText.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
            if (timeToStartRace >= 0.0f)
            {
                timeToStartRace -= Time.deltaTime;
                int timerInt = (int)timeToStartRace;
                photonView.RPC("SetTime", RpcTarget.AllBuffered, timerInt);
            }
            else if (timeToStartRace < 0.0f)
            {
                timerDone = true;
                photonView.RPC("StartTheGame", RpcTarget.AllBuffered, timerDone);
            }
        

        
    }
 
    [PunRPC]
    public void SetTime(int time)
    {
        if (time > 0.0f)
        {
            if(GetComponent<PlayerSetup>() != null && photonView.IsMine)
            {
                timeUIText.text = "Defeat the Boss";
            }

            if(GetComponent<BossSetup>() != null && photonView.IsMine)
            {
                timeUIText.text = "Defeat All Warriors";
            }
            
        }
        else
        {
            //The countdown is over
            timeUIText.text = "";
        }
    }

    [PunRPC]
    public void StartTheGame(bool canstart)
    {
        if(GetComponent<PlayerSetup>() != null && photonView.IsMine)
        {
            GetComponent<PlayerControllerPlus>().enabled = true;
        }

        if(GetComponent<BossSetup>() != null && photonView.IsMine)
        {
            GetComponent<BossController>().enabled = true;
        }
        GameStartManager.instance.canStartGame = canstart;
        this.enabled = false;
    }
}
