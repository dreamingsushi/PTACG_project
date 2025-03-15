using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.Playables;
using System.Linq;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("Login UI")] 
    public GameObject LoginUIPanel;   
    public InputField PlayerNameInput;
 
    [Header("Connecting Info Panel")]
    public GameObject ConnectingInfoUIPanel;

    [Header("GameOptions Panel")]
    public GameObject GameOptionsUIPanel;

    [Header("Create Room Panel")]
    public GameObject CreateRoomUIPanel;
    public InputField RoomNameInputField;
    public string GameMode;

    [Header("Creating Room Info Panel")]
    public GameObject CreatingRoomInfoUIPanel;

    [Header("Inside Room Panel")]
    public GameObject InsideRoomUIPanel; 
    public Text RoomInfoText;   
    public GameObject PlayerListPrefab;
    public GameObject BossListContent;
    public GameObject PlayerListContent;
    public GameObject StartGameButton;
    public GameObject lockedinButton;
    public GameObject characterSelectionPanel;
    public GameObject bossSelectionPanel;
    public GameObject joinWarriorButton;
    public GameObject joinBossButton;
    
    [Header("Map Select Room Panel")]
    public GameObject MapSelectUIRoomPanel; 
    public List<GameObject> mapSelection;
    public List<TMP_Text> voteTexts;
    public TMP_Text timerText;
    private List<int> voteCounts = new List<int>();


    // public DeathRacePlayer[] DeathRacePlayers;
    // public RacingPlayer[] RacingPlayers;
    //public GameObject[] PlayerSelectionUIGameObjects;
    private Dictionary<int, GameObject> playerListGameObjects;
    private float timeLeft = 10f;
    private int highestVotes;
    private int highestVotedMap;


    public enum Maps {
        Cave, Beach, Candy, Ruins
    }
    
    #region UNITY Methods
    // Start is called before the first frame update
    void Start()
    {
        ActivatePanel(LoginUIPanel.name);
        PhotonNetwork.AutomaticallySyncScene = true; 
        foreach(TMP_Text t in voteTexts)
        {
            voteCounts.Add(0);         
        }
        Debug.Log(voteCounts.Count);
    }

    void Update()
    {
        if(MapSelectUIRoomPanel.activeInHierarchy)
        {
            photonView.RPC("TimerDownForMapSelect", RpcTarget.AllBuffered);
            
        }
    }


    #endregion

    #region UI Callback Methods
    public void OnLoginButtonClicked()
    {
        string playerName = PlayerNameInput.text;

        if (!string.IsNullOrEmpty(playerName))
        { 
            ActivatePanel(ConnectingInfoUIPanel.name);
            if (!PhotonNetwork.IsConnected)
            {   
                PhotonNetwork.LocalPlayer.NickName = playerName;
                PhotonNetwork.ConnectUsingSettings();
            } 
        } 
        else
        {
            Debug.Log("Player name is invalid");
        }
    } 

    public void QuitButton()
	{
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();		
	}

    public void OnCancelButtonClicked()
    {
        ActivatePanel(GameOptionsUIPanel.name);
    } 

    public void OnCreateRoomButtonClicked()
    {  
        ActivatePanel(CreatingRoomInfoUIPanel.name);
        if(GameMode != null)
        {
            string roomName = RoomNameInputField.text;
            if (string.IsNullOrEmpty(roomName))
            {
                roomName = "Room" + Random.Range(1000, 10000);
            }
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 4;
    
            //The following string is used to introduce game mode
            string[] roomPropsInLobby = { "gm" }; //gm = game mode

            //two selections
            //1. warriors = "wr" 
            //2. boss = "bo"
    
            ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "gm", GameMode} };

            roomOptions.CustomRoomPropertiesForLobby = roomPropsInLobby;
            roomOptions.CustomRoomProperties = customRoomProperties;

            //In order to use this one, we need to use Photon's Realtime library
            PhotonNetwork.CreateRoom(roomName, roomOptions);  
        }     
                      
    }

    public void OnJoinRandomRoomButtonClicked()
    {
        
        //ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "gm", _gameMode }  };
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnBackButtonClicked()
    {
        ActivatePanel(GameOptionsUIPanel.name);
    }
    public void OnLeaveGameButtonClicked()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void OnStartGameButtonClicked()
    {
        //check both sides has players



        // if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("gm"))
        // {
        //     if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("wr"))
        //     {
        //         //Racing game mode
        //         PhotonNetwork.LoadLevel("RacingScene");
        //     }
        //     else if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("bo"))
        //     {
        //         //Death race mode
        //         PhotonNetwork.LoadLevel("DeathRaceScene");
        //     }
        // }
    }



    #endregion 
 
    #region Photon Callbacks
    public override void OnConnected()
    {
        Debug.Log("We connected to internet");
    }
 
    public override void OnConnectedToMaster()
    {        
        ActivatePanel(GameOptionsUIPanel.name);
        Debug.Log(PhotonNetwork.LocalPlayer.NickName+" is connected to Photon.");
    } 

    public override void OnCreatedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name+ " is created.");
    } 
 
    public override void OnJoinedRoom()
    {
        characterSelectionPanel.SetActive(false);
        bossSelectionPanel.SetActive(false);
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " joined to "+ PhotonNetwork.CurrentRoom.Name+ "Player count:"+   
                   PhotonNetwork.CurrentRoom.PlayerCount);

        ActivatePanel(InsideRoomUIPanel.name);
                 
        RoomInfoText.text = "Room name: " + PhotonNetwork.CurrentRoom.Name + " " +
                " Players " +
                PhotonNetwork.CurrentRoom.PlayerCount + " / " +
                PhotonNetwork.CurrentRoom.MaxPlayers;  


            // if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("wr"))
            // {
            //     //Racing game mode
            //     GameModeText.text = "RACE FOR FIRST!";
            //     PanelBackground.sprite = RacingBackground;    

            //     for (int i = 0; i < PlayerSelectionUIGameObjects.Length; i++)
            //     {
            //         // PlayerSelectionUIGameObjects[i].transform.Find("PlayerName").GetComponent<Text>().text = RacingPlayers[i].playerName;
            //         // PlayerSelectionUIGameObjects[i].GetComponent<Image>().sprite = RacingPlayers[i].playerSprite;
            //         // PlayerSelectionUIGameObjects[i].transform.Find("PlayerProperty").GetComponent<Text>().text = "";
            //     }
            
            // }
            // else if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsValue("bo"))
            // {
            //     //Death race game mode
            //     GameModeText.text = "RACE TO THE DEATH!";
            //     PanelBackground.sprite = DeathRaceBackground;

            //     for (int i=0; i<PlayerSelectionUIGameObjects.Length; i++)
            //     {
            //         // PlayerSelectionUIGameObjects[i].transform.Find("PlayerName").GetComponent<Text>().text =DeathRacePlayers[i].playerName;
            //         // PlayerSelectionUIGameObjects[i].GetComponent<Image>().sprite = DeathRacePlayers[i].playerSprite;
            //         // PlayerSelectionUIGameObjects[i].transform.Find("PlayerProperty").GetComponent<Text>().text = DeathRacePlayers[i].weaponName +
            //         //     ": "+  "Damage: "+ DeathRacePlayers[i].damage + " FireRate: " + DeathRacePlayers[i].fireRate;
            //     }

            // }

            if (playerListGameObjects==null)
            {
                playerListGameObjects = new Dictionary<int, GameObject>();
            }
            

            // foreach (Player player in PhotonNetwork.PlayerList)
            // {
            //     GameObject playerListGameObject = Instantiate(PlayerListPrefab);
            //     playerListGameObject.transform.SetParent(PlayerListContent.transform);
            //     playerListGameObject.transform.localScale = Vector3.one;
            //     playerListGameObject.GetComponent<PlayerListEntryInitializer>().Initialize(player.ActorNumber, player.NickName);

            //     object isPlayerReady;
            //     if (player.CustomProperties.TryGetValue(CharacterSelect.PLAYER_READY,out isPlayerReady))
            //     {
		    //         playerListGameObject.GetComponent<PlayerListEntryInitializer>().SetPlayerReady((bool)isPlayerReady);
            //     }

            //     playerListGameObjects.Add(player.ActorNumber, playerListGameObject);
            // }              
        
        StartGameButton.SetActive(false);     
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        RoomInfoText.text = "Room name: " + PhotonNetwork.CurrentRoom.Name + " " +
                  " Players/Max.Players: " +
                  PhotonNetwork.CurrentRoom.PlayerCount + " / " +
                  PhotonNetwork.CurrentRoom.MaxPlayers;

        GameObject playerListGameObject = Instantiate(PlayerListPrefab);
        playerListGameObject.transform.SetParent(PlayerListContent.transform);
        playerListGameObject.transform.localScale = Vector3.one;
       // playerListGameObject.GetComponent<PlayerListEntryInitializer>().Initialize(newPlayer.ActorNumber, newPlayer.NickName);

        playerListGameObjects.Add(newPlayer.ActorNumber, playerListGameObject);
        StartGameButton.SetActive(CheckPlayersReady());
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RoomInfoText.text = "Room name: " + PhotonNetwork.CurrentRoom.Name + " " +
                " Players/Max.Players: " +
                PhotonNetwork.CurrentRoom.PlayerCount + " / " +
                PhotonNetwork.CurrentRoom.MaxPlayers;

        Destroy(playerListGameObjects[otherPlayer.ActorNumber].gameObject);
        playerListGameObjects.Remove(otherPlayer.ActorNumber);  
        StartGameButton.SetActive(CheckPlayersReady());           
    }

    public override void OnLeftRoom()
    {
        ActivatePanel(GameOptionsUIPanel.name);

        foreach (GameObject playerListGameobject in playerListGameObjects.Values)
        {
            Destroy(playerListGameobject);
        }
        playerListGameObjects.Clear();
        playerListGameObjects = null;
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            StartGameButton.SetActive(CheckPlayersReady());
        }
    }



    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log(message); 

        //if there is no room, create one
        if (GameMode!=null)
        {
            string roomName = RoomNameInputField.text;
            if (string.IsNullOrEmpty(roomName))
            {
                roomName = "Room" + Random.Range(1000, 10000);
            }
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 3;
            string[] roomPropsInLobby = { "gm" }; //gm = game mode

            //two game modes
            //1. racing = "rc"
            //2. death race = "dr"

            ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "gm", GameMode } };

            roomOptions.CustomRoomPropertiesForLobby = roomPropsInLobby;
            roomOptions.CustomRoomProperties = customRoomProperties;

            PhotonNetwork.CreateRoom(roomName, roomOptions);
        }                     
       
    }

    public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps)
    {        
        GameObject playerListGameObject;
        if (playerListGameObjects.TryGetValue(target.ActorNumber,out playerListGameObject ))
        {
            object isPlayerReady;
            if (changedProps.TryGetValue(CharacterSelect.PLAYER_READY,out isPlayerReady ))
            {
                playerListGameObject.GetComponent<PlayerListEntryInitializer>().SetPlayerReady((bool)isPlayerReady);
            }
        }  
        StartGameButton.SetActive(CheckPlayersReady());      
    }


    #endregion 

    #region Public Methods
    public void ActivatePanel(string panelNameToBeActivated)
    {
        LoginUIPanel.SetActive(LoginUIPanel.name.Equals(panelNameToBeActivated));
        ConnectingInfoUIPanel.SetActive(ConnectingInfoUIPanel.name.Equals(panelNameToBeActivated));
        CreatingRoomInfoUIPanel.SetActive(CreatingRoomInfoUIPanel.name.Equals(panelNameToBeActivated));
        CreateRoomUIPanel.SetActive(CreateRoomUIPanel.name.Equals(panelNameToBeActivated));
        GameOptionsUIPanel.SetActive(GameOptionsUIPanel.name.Equals(panelNameToBeActivated));
        //JoinRandomRoomUIPanel.SetActive(JoinRandomRoomUIPanel.name.Equals(panelNameToBeActivated));
        InsideRoomUIPanel.SetActive(InsideRoomUIPanel.name.Equals(panelNameToBeActivated));
        MapSelectUIRoomPanel.SetActive(MapSelectUIRoomPanel.name.Equals(panelNameToBeActivated));
    }

    public void SetGameMode(string _gameMode)
    {
        GameMode = _gameMode;
    }

    public void JoinWarriorTeam()
    {
        foreach(PlayerListEntryInitializer playerObj in FindObjectsOfType<PlayerListEntryInitializer>())
        {
            if(playerObj.PlayerNameText.text == PhotonNetwork.LocalPlayer.NickName)
            {
                playerListGameObjects.Remove(PhotonNetwork.LocalPlayer.ActorNumber);
                Destroy(playerObj.gameObject);
            }
        }
        lockedinButton.GetComponent<Image>().enabled = true;
        lockedinButton.GetComponent<Button>().enabled = true;
        lockedinButton.transform.GetChild(0).gameObject.SetActive(true);
        bossSelectionPanel.SetActive(false);
        GameObject playerListedGameobject = Instantiate(PlayerListPrefab);
        playerListedGameobject.transform.SetParent(PlayerListContent.transform);
        playerListedGameobject.transform.localScale = Vector3.one;
        playerListedGameobject.GetComponent<PlayerListEntryInitializer>().Initialize(PhotonNetwork.LocalPlayer.ActorNumber, PhotonNetwork.LocalPlayer.NickName);
        joinWarriorButton.transform.SetAsLastSibling();
        characterSelectionPanel.SetActive(true);
        playerListGameObjects.Add(PhotonNetwork.LocalPlayer.ActorNumber, playerListedGameobject); 
        this.GetComponent<PlayerSelection>().playerSelectionNumber = 0;
        this.GetComponent<PlayerSelection>().ActivatePlayer(0);

        if(PlayerListContent.transform.childCount > 4)
        {
            joinWarriorButton.SetActive(false);
        }

        if(BossListContent.transform.childCount != 1)
        {
            joinBossButton.SetActive(true);
        }

        
    }

    public void JoinBossTeam()
    {
        foreach(PlayerListEntryInitializer playerObj in FindObjectsOfType<PlayerListEntryInitializer>())
        {
            if(playerObj.PlayerNameText.text == PhotonNetwork.LocalPlayer.NickName)
            {
                playerListGameObjects.Remove(PhotonNetwork.LocalPlayer.ActorNumber);

                Destroy(playerObj.gameObject);
            }     
        }
        lockedinButton.GetComponent<Image>().enabled = true;
        lockedinButton.GetComponent<Button>().enabled = true;
        lockedinButton.transform.GetChild(0).gameObject.SetActive(true);
        characterSelectionPanel.SetActive(false);
        GameObject playerListedGameobject = Instantiate(PlayerListPrefab);
        playerListedGameobject.transform.SetParent(BossListContent.transform);
        playerListedGameobject.transform.localScale = Vector3.one;
        playerListedGameobject.GetComponent<PlayerListEntryInitializer>().Initialize(PhotonNetwork.LocalPlayer.ActorNumber, PhotonNetwork.LocalPlayer.NickName);
        joinBossButton.SetActive(false);
        bossSelectionPanel.SetActive(true);
        playerListGameObjects.Add(PhotonNetwork.LocalPlayer.ActorNumber, playerListedGameobject); 
        
        this.GetComponent<PlayerSelection>().playerSelectionNumber = 3;
        

        
    }

    public void SelectMap(string map)
    {
        
        for(int i = 0; i<voteCounts.Count; i++)
        {
            mapSelection[i].SetActive(true);
            
            if(voteCounts[i] > 0)
            {
                voteCounts[i] --;
            }            
            
        }

        switch(map)
        {
            case "Cave":
                voteCounts[0]++;
                mapSelection[0].SetActive(false);
                break;
            
            case "Beach":
                voteCounts[1]++;
                mapSelection[1].SetActive(false);
                break;
            
            case "Candy":
                voteCounts[2]++;
                mapSelection[2].SetActive(false);
                break;

            case "Ruins":
                voteCounts[3]++;
                mapSelection[3].SetActive(false);
                break;
        }
    
        photonView.RPC("SyncVotes", RpcTarget.AllBuffered);
    }

    

    #endregion

    #region Private Methods
    private bool CheckPlayersReady()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return false;
        }
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            object isPlayerReady;
            if (player.CustomProperties.TryGetValue(CharacterSelect.PLAYER_READY,out isPlayerReady ))
            {
                if (!(bool)isPlayerReady)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
         return true;
    }

    [PunRPC]
    private void SyncVotes()
    {
        for(int i = 0; i<voteTexts.Count; i++)
        {
            voteTexts[i].text = voteCounts[i].ToString();
        }
    }

    [PunRPC]
    public void TimerDownForMapSelect()
    {
        if(!MapSelectUIRoomPanel.activeInHierarchy)
        {
            return;
        }
        else
        {
            timeLeft -= Time.deltaTime;
            if(timeLeft < 0)
            {
                highestVotes = voteCounts[0];
                highestVotedMap = 0;

                for(int i = 0; i<voteCounts.Count; i++)
                {
                    if(voteCounts[i] > highestVotes)
                    {
                        highestVotes = voteCounts[i];
                        highestVotedMap = i;

                    }
                    else if(voteCounts[i] == highestVotes)
                    {
                        List<int> sameVotes = new List<int>();
                        sameVotes.Add(voteCounts[i]);
                        
                        highestVotedMap = Random.Range(sameVotes[0], sameVotes.Count);
                    }
                }
                
            }
            timerText.text = timeLeft.ToString("F1");
            StartCoroutine(SelectMapStartGame());
        }   
    }

    private IEnumerator SelectMapStartGame()
    {
        if(timeLeft < 0)
        {
            switch(highestVotedMap)
            {
                case 0:
                    timerText.text = "Voted Map: Cave";
                    yield return new WaitForSeconds(4f);
                    Debug.Log("Go to Cave Scene");
                    // SceneManager.LoadScene("Cave");
                    break; 
                case 1:
                    timerText.text = "Voted Map: Beach";
                    yield return new WaitForSeconds(4f);
                    SceneManager.LoadScene("Beach");
                    break; 
                case 2:
                    timerText.text = "Voted Map: Candy";
                    Debug.Log("Go to Candy Scene");
                    break; 
                case 3:
                    timerText.text = "Voted Map: Ruins";
                    SceneManager.LoadScene("Ruins");
                    break; 
            }

            
        }
    }

    #endregion

}
