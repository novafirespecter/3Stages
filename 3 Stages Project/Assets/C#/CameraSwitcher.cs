using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    [Header("Virtual Cameras")]
    public CinemachineFreeLook freeLookCamera; // FreeLook camera
    public CinemachineVirtualCamera vCam2;     // Regular virtual camera

    [Header("Input Action")]
    public InputAction cameraSwitchAction;

    private bool isFreeLookActive = true;

    private void OnEnable()
    {
        cameraSwitchAction.Enable();
        cameraSwitchAction.performed += OnCameraSwitch;
    }

    private void OnDisable()
    {
        cameraSwitchAction.performed -= OnCameraSwitch;
        cameraSwitchAction.Disable();
    }

    private void Start()
    {
        // Initialize cameras: Start with FreeLook active
        SetCameraActive(freeLookCamera, true);
        SetCameraActive(vCam2, false);
    }

    private void OnCameraSwitch(InputAction.CallbackContext context)
    {
        // Toggle between FreeLook and the other camera
        if (isFreeLookActive)
        {
            SetCameraActive(freeLookCamera, false);
            SetCameraActive(vCam2, true);
        }
        else
        {
            SetCameraActive(freeLookCamera, true);
            SetCameraActive(vCam2, false);
        }

        isFreeLookActive = !isFreeLookActive;
    }

    private void SetCameraActive(CinemachineVirtualCameraBase camera, bool isActive)
    {
        camera.Priority = isActive ? 10 : 0;  // Higher priority activates the camera
    }
}
