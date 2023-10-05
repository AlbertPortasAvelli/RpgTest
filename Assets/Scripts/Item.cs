using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemType itemType;
    public SubItemType subItemType;
    private InventoryManager inventoryManager;
    private bool isCollected = false; // Flag to track if the item has been collected

    private void OnTriggerEnter(Collider other)
    {
        if (!isCollected && other.CompareTag("Player"))
        {
            inventoryManager = other.GetComponent<InventoryManager>();
            if (inventoryManager != null)
            {
                if (!inventoryManager.IsInventoryFull())
                {
                    int itemTypeIndex = GetItemTypeIndex(itemType, subItemType);
                    if (itemTypeIndex >= 0)
                    {
                        // Add the item to the inventory and remove it from the scene.
                        inventoryManager.AddItemToInventory(itemTypeIndex);
                        isCollected = true; // Mark the item as collected.
                        Destroy(gameObject);
                    }
                }
                else
                {
                    Debug.Log("Inventory is full. Cannot pick up item.");
                }
            }
            else
            {
                Debug.LogError("InventoryManager component not found on the player.");
            }
        }
    }

    // Helper function to get the item type index based on itemType and subItemType.
    private int GetItemTypeIndex(ItemType itemType, SubItemType subItemType)
    {
        for (int i = 0; i < inventoryManager.itemTypes.Length; i++)
        {
            InventoryItem item = inventoryManager.itemTypes[i];
            if (item.itemType == itemType && item.subItemType == subItemType)
            {
                return i;
            }
        }
        return -1; // Item type and subtype not found.
    }
}
