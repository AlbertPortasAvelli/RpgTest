using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class RightArmNightmare : MonoBehaviour
{

    public int attackDamage = 10;
    public float damageCooldown = 1.0f; // Adjust this cooldown duration as needed.

    private bool isClawTouchingPlayer = false;
    private float lastDamageTime = 0.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Time.time - lastDamageTime >= damageCooldown)
        {
            isClawTouchingPlayer = true;
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(attackDamage);
                Debug.Log("DragonNightmare left arm attacked for " + attackDamage + " damage.");
                lastDamageTime = Time.time;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isClawTouchingPlayer = false;
        }
    }

    private void Update()
    {
        if (isClawTouchingPlayer)
        {
            // Apply damage continuously while the claw is touching.
            // Subtract 10 HP or apply damage based on your game's mechanics.
        }
    }
}









