using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightmareDragon : Enemy
{

    public NightmareDragon()
    {
        maxHealth = 100;
    }
    public Collider leftArmCollider; // Declare the leftArmCollider here.
    public override void Attack()
    {
        // Check if the enemy is on cooldown.
        if (isOnCooldown)
        {
            // The enemy is on cooldown; it cannot attack yet.
            return;
        }
       
       

        animator.SetBool("isWalking", false);
        // Trigger the attack animation.
        animator.SetTrigger("IsAttacking");

        // Set the enemy on cooldown.
        isOnCooldown = true;
        // Start the coroutine to reset the animation and set the cooldown.
        StartCoroutine(ResetAnimationAndCooldown());
    }

}
