using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class InventoryItem
{
    public string itemName;
    public string itemDescription;
    public Sprite itemSprite;
    public ItemType itemType;
    public SubItemType subItemType;
}

public enum ItemType
{
    None,
    Potion,
    Armor,
    Weapon
}

public enum SubItemType
{
    None,
    HealthPotion,
    MagicPotion,
    HelmetArmor,
    ChestArmor,
    PantsArmor,
    Sword,
    Staff // Rename MagicStick to Staff for clarity.
}

public class InventoryManager : MonoBehaviour
{
    public GameObject[] inventorySlots;
    public Image[] itemImages;
    private List<int> itemsInInventory = new List<int>();

    public InventoryItem[] itemTypes; // Define your item types and associated sprites.

    public void AddItemToInventory(int itemTypeIndex)
    {
        // Find an empty slot
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (!itemsInInventory.Contains(i))
            {
                // Display the predetermined sprite for the item type on the Image component
                Image image = itemImages[i]; // Assuming itemImages[i] is the Image component in your panel

                // Set the sprite
                image.sprite = itemTypes[itemTypeIndex].itemSprite;

                // Add a BoxCollider2D component to the Image GameObject
                BoxCollider2D boxCollider = image.gameObject.AddComponent<BoxCollider2D>();

                // Set the size of the BoxCollider2D to match the dimensions of the sprite
                boxCollider.size = new Vector2(image.sprite.rect.width, image.sprite.rect.height);

                itemsInInventory.Add(i);
                return;
            }
        }
        // Inventory is full, handle accordingly
        Debug.Log("Inventory is full.");
    }


    public bool IsInventoryFull()
    {
        return itemsInInventory.Count >= inventorySlots.Length;
    }
}