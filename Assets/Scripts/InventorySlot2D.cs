using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot2D : MonoBehaviour, IDropHandler
{
    public GameObject item; // Reference to the item in the slot.

    public void OnDrop(PointerEventData eventData)
    {
        // Check if the slot is empty.
        if (!item)
        {
            // Get the dragged item from the event data.
            DraggableItem2D draggableItem = eventData.pointerDrag.GetComponent<DraggableItem2D>();
            if (draggableItem)
            {
                // Set the item in the slot and adjust its position.
                draggableItem.transform.SetParent(transform);
                draggableItem.transform.position = transform.position;
                item = draggableItem.gameObject;
            }
        }
    }
}
