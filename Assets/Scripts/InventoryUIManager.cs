using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    public GameObject inventoryUI;
    public CameraMouseLook cameraMouseLook; // Reference to the CameraMouseLook script.

    private bool isInventoryOpen = false;

    void Start()
    {
        ToggleInventory();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        inventoryUI.SetActive(isInventoryOpen);

        // Toggle camera rotation and mouse cursor behavior regardless of inventory open/close state.
        if (isInventoryOpen)
        {
           
            // Unfreeze camera rotation.
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Enable camera control.
            cameraMouseLook.EnableCameraControl();
        }
        else
        {
            // Freeze camera rotation.
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Disable camera control.
            cameraMouseLook.DisableCameraControl();
        }
    }
}
