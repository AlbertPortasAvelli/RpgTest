using UnityEngine;

public class CameraMouseLook : MonoBehaviour
{
    public float sensitivity = 2.0f; // Adjust the mouse sensitivity in the Inspector.
    public Transform playerBody; // Assign the player's GameObject in the Inspector.

    private float rotationX = 0.0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Get mouse input for camera rotation
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // Calculate camera rotation on the X-axis (up and down)
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90.0f, 90.0f);

        // Rotate the camera and the player's body
        transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
