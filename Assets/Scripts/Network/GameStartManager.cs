using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Linq;
using Unity.Mathematics;
using UnityEngine.SceneManagement;

public class GameStartManager : MonoBehaviourPunCallbacks
{
    public GameObject[] PlayerPrefabs;   
    public Transform[] InstantiatePositions; 
    public Transform bossInstantiatePosition;
    public Transform bossNextPhaseInstantiatePosition;

    public GameObject restartPanel;
    public TMP_Text TimeUIText;
    public static GameStartManager instance = null;
    public bool canStartGame;
    public float setTimeLimit = 300f;
    public float timeLeft;
    public TMP_Text timerDown;
    public GameObject Victory;
    public GameObject Defeat;
    public bool isSecondPhase;
    public bool gameResulted = false;
    public int currentDeaths;

    [Header("Stand or No Stand")]
    public bool standingPhase2;

    public GameObject DragonVictory;
    
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
                    Vector3 adjustSpawn = new Vector3(0,0,0);
                
                    int actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
                    if(actorNumber > 3)
                    {
                        adjustSpawn = new Vector3(0,5,0);
                        actorNumber = 3;
                    }
                    Vector3 instantiatePosition = InstantiatePositions[actorNumber-1].position + adjustSpawn;

                    PhotonNetwork.Instantiate(PlayerPrefabs[(int)playerSelectionNumber].name,instantiatePosition,Quaternion.identity);
                    
                }
                else if((int) playerSelectionNumber == 3)
                {
                    
                    Vector3 instantiatePosition = bossInstantiatePosition.position;
                    
                    PhotonNetwork.Instantiate(PlayerPrefabs[(int)playerSelectionNumber].name,instantiatePosition,Quaternion.identity);

                }    
                
                

            }
        }
        isSecondPhase = false;
        currentDeaths = 0;
    }

    void Update()
    {
        Debug.Log(currentDeaths);
        if(PhotonNetwork.LocalPlayer.IsMasterClient && !gameResulted && timeLeft > 0)
        {
            TimerDownForGame();
            photonView.RPC("SyncCountdownTimer", RpcTarget.AllBuffered, currentTimer, timeLeft);
        }
        else if(PhotonNetwork.LocalPlayer.IsMasterClient && timeLeft <= 0)
        {
            photonView.RPC("SyncCountdownTimer", RpcTarget.AllBuffered, "Sudden Death", 0f);
        }
        


        if(currentDeaths >= 3)
        {
            gameResulted = true;
            DragonVictory.SetActive(true);
            StartCoroutine(EndingScreen());
            
        }

        if (Input.GetKeyDown(KeyCode.Escape) && restartPanel.activeInHierarchy)
        {
            restartPanel.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !restartPanel.activeInHierarchy)
        {
            restartPanel.SetActive(true);
        }

        if(SceneManager.GetActiveScene().name == "MainMenu" || SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(SceneManager.sceneCount) || SceneManager.GetActiveScene() == SceneManager.GetSceneByBuildIndex(SceneManager.sceneCount -1))
        {
            Destroy(gameObject);
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
    public void SyncCountdownTimer(string timer, float timeRemaining)
    {
        timeRemaining = timeLeft;
        timerDown.text = timer;
    }

    public void BossNextPhase()
    {
        if(standingPhase2)
        {
            PhotonNetwork.Instantiate(PlayerPrefabs[PlayerPrefabs.Length-1].name, bossNextPhaseInstantiatePosition.transform.position, quaternion.identity);
        }
        else
            PhotonNetwork.Instantiate(PlayerPrefabs[PlayerPrefabs.Length-2].name, bossNextPhaseInstantiatePosition.transform.position, quaternion.identity);
    }

    public IEnumerator EndingScreen()
    {
        yield return new WaitForSeconds(6f);
        if(FindObjectOfType<BossHealth>().gameObject.GetComponent<PhotonView>().Owner.NickName != PhotonNetwork.LocalPlayer.NickName)
        {
            Defeat.SetActive(true);
        }
        else
            Victory.SetActive(true);

        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("WarriorDefeat");    
    }

    [PunRPC]
    public void SyncBossPhase(bool secondPhase)
    {
        isSecondPhase = secondPhase;
        Destroy(FindObjectOfType<DragonPowers>().gameObject.GetComponentInChildren<HealthBar>().gameObject);
        FindObjectOfType<DragonPowers>().gameObject.SetActive(false);

    }

    [PunRPC]
    public void IncreasePlayerDeathCount(int count)
    {
        currentDeaths = currentDeaths + count;
    }

    
    public void RestartGame()
    {

        
        PhotonNetwork.LeaveRoom();
        
        
        
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
        base.OnLeftRoom();
    }
}
