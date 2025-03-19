using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviourPunCallbacks
{
    public TMP_Text bossNickname;
    public List<TMP_Text> warriorsNickname;
    void Start()
    {
        List<string> namess = new List<string>();
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            object playerSelectionNo;
            if(player.CustomProperties.TryGetValue(CharacterSelect.PLAYER_SELECTION_NUMBER, out playerSelectionNo))
            {
                if((int)playerSelectionNo == 3)
                {
                    bossNickname.text = player.NickName;
                }
                else
                {
                    
                    namess.Add(player.NickName);
                    for(int i = 0; i<warriorsNickname.Count; i++)
                    {
                        warriorsNickname[i].text = namess[i];
                    }
                }
                
            }
        }
    }

    public void ReturnToMainMenuWorld()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
        base.OnLeftRoom();
    }
}
