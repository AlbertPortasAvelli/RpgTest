using UnityEngine;

public abstract class InteractiveItem : MonoBehaviour
{
    public float interactionRange = 3f;
    public Vector3 panelOffset = new Vector3(-1f, 0f, 0f); // Offset from the object
    public GameObject interactionPanelPrefab;
    public RectTransform crosshair; // Reference to the RectTransform of the white dot (crosshair)

    protected GameObject interactionPanelInstance;
    protected bool isInRange = false;

    private void Update()
    {
        // Perform raycasting from the position of the white dot (crosshair)
        Ray ray = Camera.main.ScreenPointToRay(crosshair.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange))
        {
            if (!isInRange)
            {
                isInRange = true;
                HighlightItem(true);
                CreateInteractionPanel();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                // Check for the "Interact" button press and perform interaction on the hit object
                InteractWithHitObject(hit.collider.gameObject);
            }
        }
        else
        {
            if (isInRange)
            {
                isInRange = false;
                HighlightItem(false);
                DestroyInteractionPanel();
            }
        }
    }

    protected abstract void Interact();

    protected void HighlightItem(bool highlight)
    {
        // Customize the highlighting effect here
        Renderer renderer = GetComponent<Renderer>(); // Assuming your object has a Renderer component

        if (renderer != null)
        {
            if (highlight)
            {
                // Apply the highlight color (e.g., white)
                renderer.material.color = Color.white;
            }
            else
            {
                // Reset the material color to its original state
                renderer.material.color = Color.gray; // You can adjust this to your desired "non-highlighted" color
            }
        }
    }

    private void CreateInteractionPanel()
    {
        if (interactionPanelPrefab != null)
        {
            // Calculate the position for the panel based on the object's position and offset
            Vector3 panelPosition = transform.position + panelOffset;

            // Create the panel
            interactionPanelInstance = Instantiate(interactionPanelPrefab, panelPosition, Quaternion.identity);

            // Set the parent of the panel to the object with the script (this)
            interactionPanelInstance.transform.SetParent(transform, false);

            // Optionally, adjust the local position to account for the offset
            interactionPanelInstance.transform.localPosition = panelOffset;
        }
    }

    private void DestroyInteractionPanel()
    {
        if (interactionPanelInstance != null)
        {
            Destroy(interactionPanelInstance);
        }
    }

    private void InteractWithHitObject(GameObject hitObject)
    {
        // Perform the interaction on the hit interactive item
        Interact();
    }
}
