using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveButtonSimon : InteractiveItem
{
    public SimonGame simonGame; // Reference to the SimonGame script

    private void Start()
    {
        // Find the SimonGame script in the scene and assign it to simonGame
        simonGame = FindObjectOfType<SimonGame>();
    }

    protected override void Interact()
    {
        Debug.Log("Hola");
        // Check if the SimonGame script reference is assigned and the game is not already playing
        if (simonGame != null && !simonGame.is_playing)
        {
            // Start the Simon Says game when the rock is interacted with
            simonGame.StartGame();
        }
    }
}
