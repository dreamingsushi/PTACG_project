using UnityEngine;
using Cinemachine;
using System.Xml.Serialization;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    [SerializeField] float mouseSense = 1;
    public Cinemachine.AxisState xAxis, yAxis;
    [SerializeField] Transform camFollowPos;
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform characterBody;
    private PlayerControllerPlus controller;



    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();


    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        controller = GetComponent<PlayerControllerPlus>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSense; // Scale X input
        float mouseY = Input.GetAxis("Mouse Y") * mouseSense; // Scale Y input

        xAxis.Value += mouseX;  // Apply sensitivity to X
        yAxis.Value -= mouseY;  // Invert Y-axis movement for natural look

        xAxis.Update(Time.deltaTime);
        yAxis.Update(Time.deltaTime);

        if (controller.currentClass == PlayerControllerPlus.PlayerClass.Mage || controller.currentClass == PlayerControllerPlus.PlayerClass.Support)
        {
            GetAimDirection();
        }

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void LateUpdate()
    {
        if (controller.isAiming)
        {
            controller.LookInfront();
            characterBody.rotation = Quaternion.Euler(0f, xAxis.Value, 0f);
            orientation.rotation = Quaternion.Euler(yAxis.Value, xAxis.Value, 0f); 
        }
        else
        {
            orientation.rotation = Quaternion.Euler(yAxis.Value, xAxis.Value, 0f); // Only rotate the camera pivot (not player)
        }
    }

    public Vector3 GetAimDirection()
    {
        Camera cam = Camera.main;
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f);
        Ray ray = cam.ScreenPointToRay(screenCenter);
        
        if (controller.currentClass == PlayerControllerPlus.PlayerClass.Mage)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, 999f, aimColliderLayerMask))
            {
                return (hit.point - controller.staffTip.position).normalized;
            }
        }

        if (controller.currentClass == PlayerControllerPlus.PlayerClass.Support)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, 999f, aimColliderLayerMask))
            {
                return (hit.point - controller.supportHand.position).normalized;
            }
        }

        return cam.transform.forward;
    }
}
