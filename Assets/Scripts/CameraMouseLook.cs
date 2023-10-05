using UnityEngine;

public class CameraMouseLook : MonoBehaviour
{
    public float sensitivity = 2.0f;
    public Transform playerBody;

    private float rotationX = 0.0f;
    private bool isCameraControlEnabled = true; // Added variable to control camera.

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (isCameraControlEnabled) // Check if camera control is enabled.
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, -90.0f, 90.0f);

            transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }

    // Public method to enable camera control.
    public void EnableCameraControl()
    {
        isCameraControlEnabled = true;
    }

    // Public method to disable camera control.
    public void DisableCameraControl()
    {
        isCameraControlEnabled = false;
    }
}
