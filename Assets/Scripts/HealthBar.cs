using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Photon.Pun.UtilityScripts;
using System.Collections.Generic;

public class HealthBar : MonoBehaviourPunCallbacks
{
    [Header("Health Bar Components")]
    [SerializeField] private Image mainHealthFill;    // Red bar (actual HP)
    [SerializeField] private Image delayedHealthFill; // Yellow bar (delayed decay effect)

    [Header("Settings")]
    [SerializeField] private float smoothSpeed = 5f;  // Speed for red bar transition
    [SerializeField] private float delaySpeed = 2f;   // Speed for yellow bar transition
    [SerializeField] private float delayTime = 0.5f;  // Delay before yellow bar shrinks
    [SerializeField] private Gradient healthColorGradient; // Health color gradient

    [SerializeField] private int playerNumber;
    [SerializeField] private TMP_Text playerName;
    private float maxHealth = 100f;
    private Coroutine delayedBarRoutine;
    public List<string> nicknames = new List<string>();
    public List<PlayerHealth> warriorHealth = new List<PlayerHealth>();

    
    void Start()
    {
        Invoke("SyncStartUI", 4f);
        
    }

    public void SyncStartUI()
    {
        nicknames.Add(PhotonNetwork.LocalPlayer.NickName);
        //playerNumber = PhotonNetwork.LocalPlayer.ActorNumber;

        var allPlayers = GameObject.FindObjectsOfType<PlayerHealth>();    
        warriorHealth.Add(this.GetComponentInParent<PlayerHealth>());
        
        foreach(PlayerHealth plyr in allPlayers)
        {
            if(plyr.GetComponent<PhotonView>().Owner.NickName != PhotonNetwork.LocalPlayer.NickName)
            {
                nicknames.Add(plyr.GetComponent<PhotonView>().Owner.NickName);
                warriorHealth.Add(plyr.GetComponent<PlayerHealth>());
            }

        }

        nicknames.Add(FindObjectOfType<BossSetup>().GetComponent<PhotonView>().Owner.NickName);


        playerName.text = nicknames[playerNumber -1];
        if(playerNumber == 4)
        {
            photonView.RPC("SyncAllHealthBarUI", RpcTarget.AllBuffered, FindObjectOfType<BossSetup>().GetComponent<BossHealth>().currentBossHP);
        }
        else
            photonView.RPC("SyncAllHealthBarUI", RpcTarget.AllBuffered, warriorHealth[playerNumber -1].currentHealth);
        
    }

    void Update()
    {   
        
        
        
        // for(int i = 0; i<GameStartManager.instance.warriors.Count; i++)
        // {
        //     if(GameStartManager.instance.warriors[i].GetComponent<PhotonView>().Owner.ActorNumber == playerNumber)
        //     {
        //         playerName.text = GameStartManager.instance.warriors[i].GetComponent<PhotonView>().Owner.NickName;
        //     }
        //     // if(GameStartManager.instance.warriors[i].GetComponent<PhotonView>().IsMine && playerNumber == 1)
        //     // {
        //     //     playerName.text = PhotonNetwork.LocalPlayer.NickName;
        //     // }
        //     // else if(!GameStartManager.instance.warriors[i].GetComponent<PhotonView>().IsMine && playerNumber == 2)
        //     // {
        //     //     playerName.text = GameStartManager.instance.warriors[i].GetComponent<PhotonView>().Owner.NickName;
        //     // }
        //     // else if(!GameStartManager.instance.warriors[i-1].GetComponent<PhotonView>().IsMine && playerNumber == 3)
        //     // {
        //     //     playerName.text = GameStartManager.instance.warriors[i].GetComponent<PhotonView>().Owner.NickName;
        //     // }
        // }
        // foreach(Player player in PhotonNetwork.PlayerList)
        // {
        //     PlayerHealth playerRef = FindObjectOfType<PlayerHealth>();
        //     if(player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        //     {
        //         playerName.text = PhotonNetwork.LocalPlayer.NickName;
        //     }
        //     else if(playerRef.gameObject.GetComponent<PhotonView>().OwnerActorNr == player.ActorNumber)
        //     {
                
        //         if(player.ActorNumber == playerNumber)
        //         {   
        //             photonView.RPC("SyncAllHealthBarUI", RpcTarget.AllBuffered, playerRef.currentHealth);
        //             playerName.text = playerRef.gameObject.GetComponent<PhotonView>().Owner.NickName;
        //         }
        //     }
        // }
        // foreach(Player player in PhotonNetwork.PlayerList)
        // {
        //     //PlayerHealth playerRef = FindObjectOfType<PlayerHealth>();
        //     if(player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        //     {
        //         playerName.text = PhotonNetwork.LocalPlayer.NickName;
        //         return;
        //     }

        //     foreach(GameObject plyr in GameStartManager.instance.warriors)
        //     {
        //         if(plyr.GetComponent<PhotonView>().Owner.ActorNumber == player.ActorNumber)
        //         {
                
        //             if(player.ActorNumber == playerNumber)
        //             {   
        //                 photonView.RPC("SyncAllHealthBarUI", RpcTarget.AllBuffered, playerRef.currentHealth);
        //                 playerName.text = playerRef.gameObject.GetComponent<PhotonView>().Owner.NickName;
        //             }
        //         }
        //     }
            
        // }

        
    }

    public void SetMaxHealth(float maxHealthValue)
    {
        maxHealth = maxHealthValue;
        mainHealthFill.fillAmount = 1f;
        delayedHealthFill.fillAmount = 1f;
        UpdateHealthBarColor(1f);
    }

   
    public void SetHealth(float newHealth)
    {
        float targetFill = Mathf.Clamp01(newHealth / maxHealth);

        mainHealthFill.fillAmount = Mathf.Clamp01(targetFill);

        if (delayedBarRoutine != null)
            StopCoroutine(delayedBarRoutine);
        StartCoroutine(SmoothHealthUpdate(mainHealthFill, targetFill, smoothSpeed));

        delayedBarRoutine = StartCoroutine(DelayedHealthUpdate(targetFill));

        UpdateHealthBarColor(targetFill);
    }


    private IEnumerator SmoothHealthUpdate(Image bar, float targetFill, float speed)
    {
        float startFill = bar.fillAmount;
        float elapsedTime = 0f;

        while (!Mathf.Approximately(bar.fillAmount, targetFill))
        {
            elapsedTime += Time.deltaTime * speed;
            bar.fillAmount = Mathf.Lerp(startFill, targetFill, elapsedTime);
            yield return null;
        }

        bar.fillAmount = targetFill;
    }

    private IEnumerator DelayedHealthUpdate(float targetFill)
    {
        yield return new WaitForSeconds(delayTime); // Wait before yellow bar starts shrinking
        yield return SmoothHealthUpdate(delayedHealthFill, targetFill, delaySpeed);
    }

    private void UpdateHealthBarColor(float healthPercentage)
    {
        mainHealthFill.color = healthColorGradient.Evaluate(healthPercentage);
    }

    [PunRPC]
    public void SyncAllHealthBarUI(int hp)
    {
        
        
        SetHealth(hp);
        
    } 
}
