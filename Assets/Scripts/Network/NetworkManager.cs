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
    public UITransitionManager UItransition;
    [Header("Options Panel")]
    public GameObject optionsPanel;
    public GameObject settingsPanel;

    [Header("Volume Panel")]
    public GameObject volumePanel;
    public GameObject graphicsPanel;

    [Header("Login UI")] 
    public GameObject LoginUIPanel;   
    public InputField PlayerNameInput;
 
    [Header("Connecting Info Panel")]
    public GameObject ConnectingInfoUIPanel;

    [Header("GameOptions Panel")]
    public GameObject GameOptionsUIPanel;
    public TMP_Text nickname;

    [Header("Create Room Panel")]
    public GameObject CreateRoomUIPanel;
    public InputField RoomNameInputField;
    public string GameMode;

    [Header("Creating Room Info Panel")]
    public GameObject CreatingRoomInfoUIPanel;

    [Header("Inside Room Panel")]
    public GameObject InsideRoomUIPanel; 
    public Text RoomInfoText;   
    public Text roomInfoText2;
    public GameObject PlayerListPrefab;
    public GameObject BossListContent;
    public GameObject PlayerListContent;
    public GameObject StartGameButton;
    public GameObject lockedinButton;
    public GameObject characterSelectionPanel;
    public GameObject bossSelectionPanel;
    public GameObject joinWarriorButton;
    public GameObject joinBossButton;
    public TMP_Text requirementsText;
    
    [Header("Map Select Room Panel")]
    public GameObject MapSelectUIRoomPanel; 
    public List<GameObject> mapSelection;
    public List<TMP_Text> voteTexts;
    public TMP_Text timerText;
    private List<int> voteCounts = new List<int>();
    private bool isMapSelecting;
    private float timeDelayToStartGame = 3f;
    private bool voted;
    private int lastVotedMap;


    // public DeathRacePlayer[] DeathRacePlayers;
    // public RacingPlayer[] RacingPlayers;
    //public GameObject[] PlayerSelectionUIGameObjects;
    private Dictionary<int, GameObject> playerListGameObjects;
    public float timeLeft = 3f;
    private int highestVotes;
    private int highestVotedMap;
    private string highestVotedMapText;


    public enum Maps {
        Island = 1, Ship = 2, Ruins = 3, Volcano = 4, Mines = 5, Space = 6, Beach = 7, Ice = 8
    }
    
    #region UNITY Methods
    // Start is called before the first frame update
    void Start()
    {
        isMapSelecting = false;
        ActivatePanel(LoginUIPanel.name);
        PhotonNetwork.AutomaticallySyncScene = true; 
        foreach(TMP_Text t in voteTexts)
        {
            voteCounts.Add(0);         
        }
    
    }

    void Update()
    {
        
        
        if(MapSelectUIRoomPanel.activeInHierarchy && PhotonNetwork.LocalPlayer.IsMasterClient && isMapSelecting)
        {
            TimerDownForMapSelect();
        }
        if(PhotonNetwork.LocalPlayer.IsMasterClient && timeLeft > 0)
        {
            photonView.RPC("SyncTimerForMapSelect", RpcTarget.AllBuffered, timeLeft.ToString("F1"));
        }
        else if(timeLeft <= 0)
        {
            photonView.RPC("SyncTimerForMapSelect", RpcTarget.AllBuffered, highestVotedMapText);

        }

        if(isMapSelecting)
        {
            ActivatePanel(MapSelectUIRoomPanel.name);
            UItransition.UpdateCamera(UItransition.mapCamera);
        }
    }


    #endregion

    #region UI Callback Methods
    public void OnLoginButtonClicked()
    {
        string playerName = PlayerNameInput.text;
        AudioManager.Instance.PlaySFX("MenuSound");

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

    public void OnLogoutButtonClicked()
    {
        if(PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
            AudioManager.Instance.PlaySFX("MenuSound");
            ActivatePanel(LoginUIPanel.name);
        }
    }

    public void OnOptionsButtonClicked()
    {
        AudioManager.Instance.PlaySFX("MenuSound");
        UItransition.UpdateCamera(UItransition.optionsCamera);
        ActivatePanel(optionsPanel.name);
        settingsPanel.SetActive(true);
    }

    public void OnVolumeButtonClicked()
    {
        OptionPanel(volumePanel.name);
        AudioManager.Instance.PlaySFX("MenuSound");
    }
    public void OnGraphicButtonClicked()
    {
        OptionPanel(graphicsPanel.name);
        AudioManager.Instance.PlaySFX("MenuSound");
    }
    public void OnLmaoButtonClicked()
    {
        AudioManager.Instance.PlaySFX("MenuSound");
    }

    public void OnReturnToGameOptionsClicked()
    {
        AudioManager.Instance.PlaySFX("MenuSound");
        UItransition.UpdateCamera(UItransition.createRoomCamera);
        ActivatePanel(GameOptionsUIPanel.name);
        settingsPanel.SetActive(false);
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
        AudioManager.Instance.PlaySFX("MenuSound");
        ActivatePanel(GameOptionsUIPanel.name);
    } 

    public void OnCreateRoomButtonClicked()
    {
        AudioManager.Instance.PlaySFX("MenuSound");
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
        AudioManager.Instance.PlaySFX("MenuSound");
        //ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "gm", _gameMode }  };
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnBackButtonClicked()
    {
        AudioManager.Instance.PlaySFX("MenuSound");
        ActivatePanel(GameOptionsUIPanel.name);
    }
    public void OnLeaveGameButtonClicked()
    {
        AudioManager.Instance.PlaySFX("MenuSound");
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
        UItransition.UpdateCamera(UItransition.createRoomCamera);
        ActivatePanel(GameOptionsUIPanel.name);
        nickname.text = PhotonNetwork.LocalPlayer.NickName;
        Debug.Log(PhotonNetwork.LocalPlayer.NickName+" is connected to Photon.");
    } 

    public override void OnCreatedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name+ " is created.");
        PhotonNetwork.EnableCloseConnection = true;
    } 
 
    public override void OnJoinedRoom()
    {
        UItransition.UpdateCamera(UItransition.lobbyCamera);
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " joined to "+ PhotonNetwork.CurrentRoom.Name+ "Player count:"+   
                   PhotonNetwork.CurrentRoom.PlayerCount);

        ActivatePanel(InsideRoomUIPanel.name);
                 
        RoomInfoText.text = "Room name: " + PhotonNetwork.CurrentRoom.Name;
        roomInfoText2.text =  " Players " +
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
            

            foreach (Player player in PhotonNetwork.PlayerList)
            {
                
                        GameObject playerListGameObject = PhotonNetwork.Instantiate(PlayerListPrefab.name, Vector3.zero, Quaternion.identity);
                        playerListGameObject.transform.SetParent(PlayerListContent.transform);
                        playerListGameObject.transform.localScale = Vector3.one;
                        playerListGameObject.GetComponent<PlayerListEntryInitializer>().Initialize(player.ActorNumber, player.NickName);
                        //playerListGameObject.GetComponent<PhotonView>().RPC("ChangeClassName", RpcTarget.AllBuffered, playerListGameObject.GetComponent<PlayerListEntryInitializer>().SelectedClass.text);
                        object isPlayerReady;
                        if (player.CustomProperties.TryGetValue(CharacterSelect.PLAYER_READY,out isPlayerReady))
                        {
                            object playerSelectionNo;
                            if(player.CustomProperties.TryGetValue(CharacterSelect.PLAYER_SELECTION_NUMBER, out playerSelectionNo))
                            {
                                playerListGameObject.GetComponent<PhotonView>().RPC("ChangeClassName", RpcTarget.AllBuffered, GetComponent<PlayerSelection>().SelectablePlayers[(int)playerSelectionNo].name);
                            }
                            playerListGameObject.GetComponent<PlayerListEntryInitializer>().SetPlayerReady((bool)isPlayerReady);
                            

                        }

                        playerListGameObjects.Add(player.ActorNumber, playerListGameObject);
                        Debug.Log("Players in Lobby" + playerListGameObjects.Count);
                  
                
            }              
        
        StartGameButton.SetActive(false);     
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        RoomInfoText.text = "Room name: " + PhotonNetwork.CurrentRoom.Name;
        roomInfoText2.text =  " Players " +
                PhotonNetwork.CurrentRoom.PlayerCount + " / " +
                PhotonNetwork.CurrentRoom.MaxPlayers; 

        GameObject playerListGameObject = PhotonNetwork.Instantiate(PlayerListPrefab.name, Vector3.zero, Quaternion.identity);
        playerListGameObject.transform.SetParent(PlayerListContent.transform);
        playerListGameObject.transform.localScale = Vector3.one;
        playerListGameObject.GetComponent<PlayerListEntryInitializer>().Initialize(newPlayer.ActorNumber, newPlayer.NickName);
        //playerListGameObject.GetComponent<PhotonView>().RPC("ChangeClassName", RpcTarget.AllBuffered, GetComponent<PlayerSelection>().SelectablePlayers[GetComponent<PlayerSelection>().playerSelectionNumber].name);
        playerListGameObjects.Add(newPlayer.ActorNumber, playerListGameObject);
        StartGameButton.SetActive(CheckPlayersReady());
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RoomInfoText.text = "Room name: " + PhotonNetwork.CurrentRoom.Name;
        roomInfoText2.text =  " Players " +
                PhotonNetwork.CurrentRoom.PlayerCount + " / " +
                PhotonNetwork.CurrentRoom.MaxPlayers; 

        Destroy(playerListGameObjects[otherPlayer.ActorNumber].gameObject);
        playerListGameObjects.Remove(otherPlayer.ActorNumber);  
        StartGameButton.SetActive(CheckPlayersReady());           
    }

    public override void OnLeftRoom()
    {
        UItransition.UpdateCamera(UItransition.createRoomCamera);
        
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
                roomName = "Colosseum" + Random.Range(1000, 10000);
            }
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 4;
            //string[] roomPropsInLobby = { "gm" }; //gm = game mode

            //two game modes
            //1. racing = "rc"
            //2. death race = "dr"

            // ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "gm", GameMode } };

            // roomOptions.CustomRoomPropertiesForLobby = roomPropsInLobby;
            // roomOptions.CustomRoomProperties = customRoomProperties;

            PhotonNetwork.CreateRoom(roomName, roomOptions);
        }                     
       
    }

    public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps)
    {        
        GameObject playerListGameObject;
        if (playerListGameObjects.TryGetValue(target.ActorNumber,out playerListGameObject ))
        {
            object playerSelectionNo;
            if(changedProps.TryGetValue(CharacterSelect.PLAYER_SELECTION_NUMBER, out playerSelectionNo))
            {
                playerListGameObject.GetComponent<PhotonView>().RPC("ChangeClassName", RpcTarget.AllBuffered, GetComponent<PlayerSelection>().SelectablePlayers[(int)playerSelectionNo].name);
                if((int)playerSelectionNo == 3)
                {
                    Debug.Log("u picked dragon");
                }
            }
            
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

    public void OptionPanel(string settingsToBeActivated)
    {
        volumePanel.SetActive(volumePanel.name.Equals(settingsToBeActivated));
        graphicsPanel.SetActive(graphicsPanel.name.Equals(settingsToBeActivated));
    }

    public void GoToMapSelect()
    {
        AudioManager.Instance.PlaySFX("MenuSound");
        photonView.RPC("SyncMapSelect", RpcTarget.AllBuffered, true);
        
    }

    [PunRPC]
    public void SyncMapSelect(bool isTrue)
    {
        isMapSelecting = isTrue;
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


            GameObject playerListedGameobject = PhotonNetwork.Instantiate("PlayersInLobbyPrefab", PlayerListContent.transform.position, Quaternion.identity);
            
            playerListedGameobject.transform.SetParent(PlayerListContent.transform, true);
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
            GameObject playerListedGameobject = PhotonNetwork.Instantiate("PlayersInLobbyPrefab", PlayerListContent.transform.position, Quaternion.identity);
            PhotonView photonView = playerListedGameobject.GetComponent<PhotonView>();
            photonView.TransferOwnership(PhotonNetwork.LocalPlayer);

            playerListedGameobject.transform.SetParent(BossListContent.transform, true);   
            playerListedGameobject.transform.localScale = Vector3.one;
            playerListedGameobject.GetComponent<PlayerListEntryInitializer>().Initialize(PhotonNetwork.LocalPlayer.ActorNumber, PhotonNetwork.LocalPlayer.NickName);
            joinBossButton.SetActive(false);
            bossSelectionPanel.SetActive(true);
            playerListGameObjects.Add(PhotonNetwork.LocalPlayer.ActorNumber, playerListedGameobject); 
            
            this.GetComponent<PlayerSelection>().playerSelectionNumber = 3;
        
    
    }

    // public void SyncWarriors()
    // {
    //     photonView.RPC("JoinWarriorTeam", RpcTarget.AllBuffered);
    // }

    // public void SyncBoss()
    // {
    //     photonView.RPC("JoinBossTeam", RpcTarget.AllBuffered);
    // }

    public void SelectMap(int mapIndex)
    {
        
        
        for(int i = 0; i<voteCounts.Count; i++)
        {
            mapSelection[i].SetActive(true);                     
            
        }



        switch((Maps)mapIndex)
        {
            case Maps.Island:
                for(int i = 0; i< voteCounts.Count; i++)
                {
                    if(mapIndex-1 == i)
                    {
                        voteCounts[i]++;
                    }
                    
                    
                }
                
                foreach(Player player in PhotonNetwork.PlayerList)
                {
                    if(player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                    {
                        mapSelection[mapIndex-1].SetActive(false);
                        if(voted)
                        {
                            voteCounts[lastVotedMap-1] --;
                        }
                    }
                }
                
                for(int j = 0; j<voteCounts.Count; j++)
                {
                    if(voteCounts[j] <0)
                    {
                        voteCounts[j] = 0;
                    }
                    photonView.RPC("SyncVotes", RpcTarget.AllBuffered, j, voteCounts[j]);    
                    
                }
                break;
            
            case Maps.Ship:
                for(int i = 0; i< voteCounts.Count; i++)
                {
                    if(mapIndex-1 == i)
                    {
                        voteCounts[i]++;
                    }
                    
                }
                
                foreach(Player player in PhotonNetwork.PlayerList)
                {
                    if(player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                    {
                        mapSelection[mapIndex-1].SetActive(false);
                        if(voted)
                        {
                            voteCounts[lastVotedMap-1] --;
                        }
                    }
                }
                
                for(int j = 0; j<voteCounts.Count; j++)
                {
                    if(voteCounts[j] <0)
                    {
                        voteCounts[j] = 0;
                    }
                    photonView.RPC("SyncVotes", RpcTarget.AllBuffered, j, voteCounts[j]);    
                    
                }
                break;
            
            case Maps.Ruins:
                for(int i = 0; i< voteCounts.Count; i++)
                {
                    if(mapIndex-1 == i)
                    {
                        voteCounts[i]++;
                    }
                    
                }
                
                foreach(Player player in PhotonNetwork.PlayerList)
                {
                    if(player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                    {
                        mapSelection[mapIndex-1].SetActive(false);
                        if(voted)
                        {
                            voteCounts[lastVotedMap-1] --;
                        }
                    }
                }
                
                for(int j = 0; j<voteCounts.Count; j++)
                {
                    if(voteCounts[j] <0)
                    {
                        voteCounts[j] = 0;
                    }
                    photonView.RPC("SyncVotes", RpcTarget.AllBuffered, j, voteCounts[j]);    
                    
                }
                break;

            case Maps.Volcano:
                for(int i = 0; i< voteCounts.Count; i++)
                {
                    if(mapIndex-1 == i)
                    {
                        voteCounts[i]++;
                    }
                    
                }
                
                foreach(Player player in PhotonNetwork.PlayerList)
                {
                    if(player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                    {
                        mapSelection[mapIndex-1].SetActive(false);
                        if(voted)
                        {
                            voteCounts[lastVotedMap-1] --;
                        }
                    }
                }
                
                for(int j = 0; j<voteCounts.Count; j++)
                {
                    if(voteCounts[j] <0)
                    {
                        voteCounts[j] = 0;
                    }
                    photonView.RPC("SyncVotes", RpcTarget.AllBuffered, j, voteCounts[j]);    

                }
                break;

            case Maps.Mines:
                for(int i = 0; i< voteCounts.Count; i++)
                {
                    if(mapIndex-1 == i)
                    {
                        voteCounts[i]++;
                    }
                    
                }
                
                foreach(Player player in PhotonNetwork.PlayerList)
                {
                    if(player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                    {
                        mapSelection[mapIndex-1].SetActive(false);
                        if(voted)
                        {
                            voteCounts[lastVotedMap-1] --;
                        }
                    }
                }
                
                for(int j = 0; j<voteCounts.Count; j++)
                {
                    if(voteCounts[j] <0)
                    {
                        voteCounts[j] = 0;
                    }
                    photonView.RPC("SyncVotes", RpcTarget.AllBuffered, j, voteCounts[j]);    

                }
                break;
            
            case Maps.Space:
                for(int i = 0; i< voteCounts.Count; i++)
                {
                    if(mapIndex-1 == i)
                    {
                        voteCounts[i]++;
                    }
                    
                }
                
                foreach(Player player in PhotonNetwork.PlayerList)
                {
                    if(player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                    {
                        mapSelection[mapIndex-1].SetActive(false);
                        if(voted)
                        {
                            voteCounts[lastVotedMap-1] --;
                        }
                    }
                }
                
                for(int j = 0; j<voteCounts.Count; j++)
                {
                    if(voteCounts[j] <0)
                    {
                        voteCounts[j] = 0;
                    }
                    photonView.RPC("SyncVotes", RpcTarget.AllBuffered, j, voteCounts[j]);    

                }
                break;
            
            case Maps.Beach:
                for(int i = 0; i< voteCounts.Count; i++)
                {
                    if(mapIndex-1 == i)
                    {
                        voteCounts[i]++;
                    }
                    
                }
                
                foreach(Player player in PhotonNetwork.PlayerList)
                {
                    if(player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                    {
                        mapSelection[mapIndex-1].SetActive(false);
                        if(voted)
                        {
                            voteCounts[lastVotedMap-1] --;
                        }
                    }
                }
                
                for(int j = 0; j<voteCounts.Count; j++)
                {
                    if(voteCounts[j] <0)
                    {
                        voteCounts[j] = 0;
                    }
                    photonView.RPC("SyncVotes", RpcTarget.AllBuffered, j, voteCounts[j]);    

                }
                break;

            case Maps.Ice:
                for(int i = 0; i< voteCounts.Count; i++)
                {
                    if(mapIndex-1 == i)
                    {
                        voteCounts[i]++;
                    }
                    
                }
                
                foreach(Player player in PhotonNetwork.PlayerList)
                {
                    if(player.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                    {
                        mapSelection[mapIndex-1].SetActive(false);
                        if(voted)
                        {
                            voteCounts[lastVotedMap-1] --;
                        }
                    }
                }
                
                for(int j = 0; j<voteCounts.Count; j++)
                {
                    if(voteCounts[j] <0)
                    {
                        voteCounts[j] = 0;
                    }
                    photonView.RPC("SyncVotes", RpcTarget.AllBuffered, j, voteCounts[j]);    

                }
                break;
        }
        
        lastVotedMap = mapIndex;
        
        voted = true;
        
    }

    

    #endregion

    #region Private Methods
    private bool CheckPlayersReady()
    {
        int sameDragon = 0;
        if (!PhotonNetwork.IsMasterClient)
        {
            return false;
        }
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            object playerSelectionNo;
                
            if(player.CustomProperties.TryGetValue(CharacterSelect.PLAYER_SELECTION_NUMBER, out playerSelectionNo))
            {
                if((int)playerSelectionNo == 3)
                {
                    sameDragon++;
                    
                }
            }
        

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

        if(sameDragon == 0)
        {
            Debug.Log("Need at Least 1 Dragon");
            requirementsText.text = "One Dragon is required to start";
            return false;
        }
        else if(sameDragon > 1)
        {
            Debug.Log("Can only have 1 Dragon");
            requirementsText.text = "There are too many dragons";
            return false;
        }
        requirementsText.text = "";
        return true;      
    }

    [PunRPC]
    public void SyncVotes(int index, int voteCountNum)
    {
        voteCounts[index] = voteCountNum;
        voteTexts[index].text = voteCountNum.ToString();
        
            
        
    }

    
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
                isMapSelecting = false;
                highestVotes = voteCounts[0];
                highestVotedMap = 0;

                List<int> sameVotes = new List<int>();
                for(int i = 0; i<voteCounts.Count; i++)
                {
                    
                    if(voteCounts[i] > highestVotes)
                    {
                        highestVotes = voteCounts[i];
                        highestVotedMap = i;
                        sameVotes.Clear();
                        sameVotes.Add(i);
                    }
                    else if(voteCounts[i] == highestVotes)
                    {
                        
                        sameVotes.Add(i);    
                        
                    }
                    
                }   

                if(sameVotes.Count > 1)
                {
                    highestVotedMap = Random.Range(0, sameVotes.Count);
                }    
                
            }
            
            timerText.text = timeLeft.ToString("F1");
            StartCoroutine(SelectMapStartGame());
        }   
    }

    [PunRPC]
    public void SyncTimerForMapSelect(string timerLeftText)
    {
        timerText.text = timerLeftText;
    }

    private IEnumerator SelectMapStartGame()
    {
        if(timeLeft < 0)
        {
            AudioManager.Instance.PlayMusic("BattleBGM");
            switch(highestVotedMap)
            {
                case 0:
                    highestVotedMapText = "Voted Map: Island";
                    yield return new WaitForSeconds(timeDelayToStartGame);
                    
                    SceneManager.LoadScene("Ruins");
                    break; 
                case 1:
                    highestVotedMapText = "Voted Map: Ship";
                    yield return new WaitForSeconds(timeDelayToStartGame);
                    
                    SceneManager.LoadScene("Damian_Castle");
                    break; 
                case 2:
                    highestVotedMapText = "Voted Map: Ruins";
                    yield return new WaitForSeconds(timeDelayToStartGame);
                    
                    SceneManager.LoadScene("Stronghold");
                    break; 
                case 3:
                    highestVotedMapText = "Voted Map: Volcano";
                    yield return new WaitForSeconds(timeDelayToStartGame);
                    
                    SceneManager.LoadScene("Alvin_Scene 2");
                    break; 
                case 4:
                    highestVotedMapText = "Voted Map: Mines";
                    yield return new WaitForSeconds(timeDelayToStartGame);
                   
                    SceneManager.LoadScene("Alvin_Scene");
                    break; 
                case 5:
                    highestVotedMapText = "Voted Map: Space";
                    yield return new WaitForSeconds(timeDelayToStartGame);
                  
                    SceneManager.LoadScene("Damian_Space");
                    break; 
                case 6:
                    highestVotedMapText = "Voted Map: Beach";
                    yield return new WaitForSeconds(timeDelayToStartGame);
      
                    SceneManager.LoadScene("Beach");
                    break; 
                case 7:
                    highestVotedMapText = "Voted Map: Ice Cliffs";
                    yield return new WaitForSeconds(timeDelayToStartGame);
   
                    SceneManager.LoadScene("Snow");
                    break; 
            }
        }
    }

    #endregion

    public void LoadruinsNow()
    {
        SceneManager.LoadScene("Ruins");
    }
}
