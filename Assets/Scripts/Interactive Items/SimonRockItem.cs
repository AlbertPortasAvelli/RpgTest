using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonRockItem : InteractiveItem
{
    // Any specific properties or fields for SimonRockItem can be added here

    protected override void Interact()
    {
        // Implement the interaction logic for the SimonRockItem here
        // This method will be called when the player interacts with this specific item
        Debug.Log("Interacted with a Simon Rock.");
    }
}
