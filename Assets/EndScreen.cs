using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public void ReturnToMainMenuWorld()
    {
        SceneManager.LoadScene(0);
    }
}
