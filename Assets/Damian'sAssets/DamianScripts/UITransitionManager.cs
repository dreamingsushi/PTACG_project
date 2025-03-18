using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITransitionManager : MonoBehaviour
{
    public CinemachineVirtualCamera currentCamera;
    public CinemachineVirtualCamera loginCamera;
    public CinemachineVirtualCamera createRoomCamera;
    public CinemachineVirtualCamera optionsCamera;
    public CinemachineVirtualCamera lobbyCamera;
    public CinemachineVirtualCamera mapCamera;



    public void Start()
    {
        ///currentCamera.Priority++;
    }

    public void UpdateCamera(CinemachineVirtualCamera target)
    {
        currentCamera.Priority--;

        currentCamera = target;

        currentCamera.Priority++;

        //Debug.Log (target);
    }
}
