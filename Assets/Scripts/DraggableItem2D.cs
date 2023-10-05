using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableItem2D : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 originalPosition;
    private Transform originalParent; // Store the original parent (the slot).

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        originalPosition = rectTransform.localPosition;
        originalParent = transform.parent; // Store the original parent (the slot).
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        
        canvasGroup.blocksRaycasts = false;

        // Set the item's parent to the Canvas so it's not constrained by the slot.
        transform.SetParent(transform.root);

        // Optionally, bring the item to the front.
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("DRAggin");
        
        rectTransform.anchoredPosition += eventData.delta / canvasGroup.transform.localScale.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // Check if the item was dropped over a valid inventory slot.
        InventorySlot2D[] slots = FindObjectsOfType<InventorySlot2D>();
        foreach (InventorySlot2D slot in slots)
        {
            RectTransform slotRect = slot.GetComponent<RectTransform>();
            if (RectTransformUtility.RectangleContainsScreenPoint(slotRect, eventData.position))
            {
                // The item was dropped over a valid slot, so snap it to the slot's position.
                rectTransform.SetParent(slot.transform); // Change the parent to the new slot.
                rectTransform.anchoredPosition = Vector2.zero; // Snap to the center of the slot.
                return; // Exit the loop.
            }
        }

        // If the item was not dropped over a valid slot, return it to its original parent and position.
        rectTransform.SetParent(originalParent);
        rectTransform.localPosition = originalPosition;
    }
}
