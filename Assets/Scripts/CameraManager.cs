using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] float mouseSense = 1;
    public Cinemachine.AxisState xAxis, yAxis;
    [SerializeField] Transform camFollowPos;
    [SerializeField] private Transform orientation;
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSense; // Scale X input
        float mouseY = Input.GetAxis("Mouse Y") * mouseSense; // Scale Y input

        xAxis.Value += mouseX;  // Apply sensitivity to X
        yAxis.Value -= mouseY;  // Invert Y-axis movement for natural look

        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);
    }

    private void LateUpdate()
    {
        orientation.rotation = Quaternion.Euler(yAxis.Value, xAxis.Value, 0f); // Only rotate the camera pivot (not player)
    }
}
