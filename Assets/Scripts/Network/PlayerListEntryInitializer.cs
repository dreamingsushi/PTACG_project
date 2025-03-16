using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PlayerListEntryInitializer : MonoBehaviourPunCallbacks
{
    [Header("UI References")]
    public TMP_Text PlayerNameText;
    public Button PlayerReadyButton;
    public Image PlayerReadyImage;
    public TMP_Text SelectedClass;

    private bool isPlayerReady = false;

    void Update()
    {
        
        

        
    }

    public void Initialize(int playerID, string playerName)
    {
        PlayerNameText.text = playerName;
        if (PhotonNetwork.LocalPlayer.ActorNumber != playerID)
        {
            PlayerReadyButton.gameObject.SetActive(false);
        }
        else
        {
            //I am the local player
            ExitGames.Client.Photon.Hashtable initialProps = new ExitGames.Client.Photon.Hashtable(){{CharacterSelect.PLAYER_READY,isPlayerReady}};
            PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);
            
            PlayerReadyButton.onClick.AddListener(() =>
            {
                photonView.RPC("ChangeClassName", RpcTarget.AllBuffered, SelectedClass.text);
                isPlayerReady = !isPlayerReady;
                SetPlayerReady(isPlayerReady);

                ExitGames.Client.Photon.Hashtable newProps = new ExitGames.Client.Photon.Hashtable(){{CharacterSelect.PLAYER_READY,isPlayerReady}};
                PhotonNetwork.LocalPlayer.SetCustomProperties(newProps);
            });
        }


    }

    public void SetPlayerReady(bool playerReady)
    {
        PlayerReadyImage.enabled = playerReady;

        if (playerReady == true)
        {
            PlayerReadyButton.GetComponentInChildren<TMP_Text>().text = "Change Character";
        }
        else
        {
            PlayerReadyButton.GetComponentInChildren<TMP_Text>().text = "Lock in";
        }
    }

    [PunRPC]
    public void ChangeClassName(string classText)
    {
        SelectedClass.text = classText;
            // object playerSelectionNumber;
            // if(PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(CharacterSelect.PLAYER_SELECTION_NUMBER, out playerSelectionNumber))
            // {
                
            //     switch((int)playerSelectionNumber)
            //     {
            //         case 0:
            //             SelectedClass.text = classText;
            //             break;
            //             case 1:
            //             SelectedClass.text = classText;
            //             break;
            //             case 2:
            //             SelectedClass.text = classText;
            //             break;
            //             case 3:
            //             SelectedClass.text = classText;
            //             break;
            //     }
                
            // } 
            
               
    }

    

}
