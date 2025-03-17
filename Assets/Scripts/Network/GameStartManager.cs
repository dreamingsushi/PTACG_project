using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Linq;
using Unity.Mathematics;

public class GameStartManager : MonoBehaviourPunCallbacks
{
    public GameObject[] PlayerPrefabs;   
    public Transform[] InstantiatePositions; 
    public Transform bossInstantiatePosition;
    public Transform bossNextPhaseInstantiatePosition;

    public TMP_Text TimeUIText;
    public static GameStartManager instance = null;
    public bool canStartGame;
    public float setTimeLimit = 300f;
    public float timeLeft;
    public TMP_Text timerDown;
    public GameObject Victory;
    public GameObject Defeat;
    public bool gameResulted;
    private string currentTimer;

    void Awake()
    {
        //check if instance already exists
        if (instance == null)
        {
            instance = this;
        }

        //If not
        else if (instance != this)
        {
            //Then, destroy this. This enforces our singleton pattern, meaning that there can only ever be one instance of a GameManager
            Destroy(gameObject);
        }

        //Don't destroy when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = setTimeLimit;
        if (PhotonNetwork.IsConnectedAndReady)
        {
            object playerSelectionNumber;
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(CharacterSelect.PLAYER_SELECTION_NUMBER, out playerSelectionNumber ))
            {
                if((int) playerSelectionNumber != 3)
                {
                    Debug.Log((int)playerSelectionNumber);
                
                    int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
                    Vector3 instantiatePosition = InstantiatePositions[actorNumber-1].position;

                    PhotonNetwork.Instantiate(PlayerPrefabs[(int)playerSelectionNumber].name,instantiatePosition,Quaternion.identity);
                }
                else if((int) playerSelectionNumber == 3)
                {
                    Debug.Log((int)playerSelectionNumber);
                    Vector3 instantiatePosition = bossInstantiatePosition.position;
                    PhotonNetwork.Instantiate(PlayerPrefabs[(int)playerSelectionNumber].name,instantiatePosition,Quaternion.identity);
                }    
                
                

            }
        }

       
    }

    void Update()
    {
        
        if(PhotonNetwork.LocalPlayer.IsMasterClient && !gameResulted && timeLeft > 0)
        {
            TimerDownForGame();
            photonView.RPC("SyncCountdownTimer", RpcTarget.AllBuffered, currentTimer);
        }

        if(timeLeft <= 0)
        {
            photonView.RPC("SyncCountdownTimer", RpcTarget.AllBuffered, "Sudden Death");
        }
        
    }


    public void TimerDownForGame()
    {
        if(!canStartGame)
        {
            return;
        }
        else
        {
            timeLeft -= Time.deltaTime;
            float minutes = Mathf.FloorToInt(timeLeft / 60);
            float seconds = Mathf.FloorToInt(timeLeft % 60);

            if(timeLeft < setTimeLimit/2)
            {
                Debug.Log("time left is halved");
                
            }
            currentTimer = string.Format("{0:00}:{1:00}", minutes, seconds);
            
        }   
    }

    [PunRPC]
    public void SyncCountdownTimer(string timer)
    {
        timerDown.text = timer;
    }

    public void BossNextPhase()
    {
        PhotonNetwork.Instantiate(PlayerPrefabs[PlayerPrefabs.Length-1].name, bossNextPhaseInstantiatePosition.transform.position, quaternion.identity);
    }

}
