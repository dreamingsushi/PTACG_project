using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Linq;
using Unity.Mathematics;

public class GameStartManager : MonoBehaviour
{
    public GameObject[] PlayerPrefabs;   
    public Transform[] InstantiatePositions; 
    public Transform bossInstantiatePosition;
    public Transform bossNextPhaseInstantiatePosition;

    public TMP_Text TimeUIText;
    public static GameStartManager instance = null;

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
                else
                {
                    Debug.Log((int)playerSelectionNumber);
                    Vector3 instantiatePosition = bossInstantiatePosition.position;
                    PhotonNetwork.Instantiate(PlayerPrefabs[(int)playerSelectionNumber].name,instantiatePosition,Quaternion.identity);
                }    
                
                

            }
        }

       
    }

    public void BossNextPhase()
    {
        Instantiate(PlayerPrefabs[PlayerPrefabs.Length], bossNextPhaseInstantiatePosition.transform.position, quaternion.identity);
    }

}
