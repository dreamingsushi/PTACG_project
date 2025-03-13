using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PlayerListEntryInitializer : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text PlayerNameText;
    public Button PlayerReadyButton;
    public Image PlayerReadyImage;

    private bool isPlayerReady = false;

    public void Awake()
    {

        PlayerReadyButton = GameObject.Find("lockedin").GetComponent<Button>();
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
            //ExitGames.Client.Photon.Hashtable initialProps = new ExitGames.Client.Photon.Hashtable(){{MultiplayerRacingGame.PLAYER_READY,isPlayerReady}};
            //PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);

            PlayerReadyButton.onClick.AddListener(() =>
            {
                isPlayerReady = !isPlayerReady;
                SetPlayerReady(isPlayerReady);

                //ExitGames.Client.Photon.Hashtable newProps = new ExitGames.Client.Photon.Hashtable(){{MultiplayerRacingGame.PLAYER_READY,isPlayerReady}};
                //PhotonNetwork.LocalPlayer.SetCustomProperties(newProps);
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

}
