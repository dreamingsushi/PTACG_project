using UnityEngine;
using Cinemachine;

public class UIBillboarding : MonoBehaviour
{
    private Transform camTransform;

    private void Start()
    {
        // Get the active camera from the CinemachineBrain
        CinemachineBrain brain = Camera.main.GetComponent<CinemachineBrain>();
        if (brain != null)
        {
            camTransform = brain.OutputCamera.transform;
        }
        else
        {
            Debug.LogError("No Cinemachine Brain found on the main camera!");
        }
    }

    private void LateUpdate()
    {
        if (camTransform != null)
        {
            transform.forward = camTransform.forward;
        }
    }
}
