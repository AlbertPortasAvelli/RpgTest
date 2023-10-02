using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpForce = 8f;
    public float speed = 5f;
    public float rotationSpeed = 120f;
    public float groundCheckDistance = 0.2f;
    public float minAirTimeBeforeJumpDown = 0.7f;
    public float attackDuration = 0.5f;
    public int defaultDamageAmount = 20; // The damage amount to apply to enemies.
    public float attackRange = 2.0f;
    public int maxHealth = 100;
    private int currentHealth;

    private bool isJumpingUp = false;
    private bool isJumpingDown = false;
    private float jumpStartTime;
    private bool isGrounded = true;
    private bool hasAttackedThisClick = false; // Flag to track if the player has attacked during the current click.
    private bool isInvulnerable = false; // Flag to track if the player is invulnerable during attack animations.





    private Rigidbody rb;
    private Animator animator;
    public LayerMask groundLayer;
    public LayerMask enemyLayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    private void Update()
    {
        // Check if the player is grounded.
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

        // Handle jump input.
        if (Input.GetButtonDown("Jump"))
        {
            JumpUp();
           
        }

        
            float moveVertical = Input.GetAxis("Vertical");
            Vector3 movement = transform.forward * moveVertical * speed;

            // Handle rotation using "A" and "D" keys
            float rotationInput = Input.GetAxis("Horizontal");
            transform.Rotate(Vector3.up * rotationInput * rotationSpeed * Time.deltaTime);

            // Apply movement to the rigidbody
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

            // Check if the character is walking
            bool isWalking = Mathf.Abs(moveVertical) > 0.1f;

            // Update the Animator's "isRunning" parameter
            animator.SetBool("isRunning", isWalking);
            if (!isWalking)
            {
                isWalking = false;
                animator.SetBool("isRunning", isWalking);
               
            }
        
        
            
            



        if (isJumpingUp && !isJumpingDown)
        {
            // Check for the raycast and trigger "Jump Down" animation.
            if (!isGrounded)
            {
                // Check if we've been in the air for a minimum time before transitioning to "Jump Down."
                if (Time.time - jumpStartTime >= minAirTimeBeforeJumpDown)
                {
                    StartJumpDown();
                    FallGroundCheker();
                }
            }
        }

        if(!isJumpingUp && !isJumpingDown)
        {
            // Check for left-click (you can customize this input)
            if (Input.GetMouseButtonDown(0) && !hasAttackedThisClick)
            {
                // Trigger the attack animation
                Attack();
                Debug.Log("isAttacking : " + animator.GetBool("isAttacking"));
                // Start a coroutine to reset isAttacking after a delay
                StartCoroutine(ResetAttackState());
                hasAttackedThisClick = true;
            }
        }
    }

    private void JumpUp()
    {
        // Apply an upward force to perform the jump.
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        // Start "Jump Up" animation.
        isJumpingUp = true;
        isJumpingDown = false;
        jumpStartTime = Time.time; // Record the time when the jump started.
        // Set animator parameters.
        animator.SetBool("isJumpingUp", isJumpingUp);
        animator.SetBool("isJumpingDown", isJumpingDown);
    }

    private void StartJumpDown()
    {
        // Start "Jump Down" animation.
        isJumpingUp = false;
        isJumpingDown = true;
        // Set animator parameters.
        animator.SetBool("isJumpingUp", isJumpingUp);
        animator.SetBool("isJumpingDown", isJumpingDown);
    }
    IEnumerator FallGroundCheker()
    {
        yield return new WaitForSeconds(0.1f);
        isJumpingUp = false;
        isJumpingDown = false;
        animator.SetBool("isJumpingUp", isJumpingUp);
        animator.SetBool("isJumpingDown", isJumpingDown);

    }
    // Function to trigger the attack animation
    private void Attack()
    {
        
        // Trigger the "Attack" animation parameter in the Animator
        animator.SetBool("isAttacking", true);
        // Find the nearest enemy within the attack range.
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);
        Enemy nearestEnemy = null;
        float nearestDistance = float.MaxValue;

        foreach (Collider collider in hitColliders)
        {
            // Check if the collided object has an "Enemy" tag (you can set this in the Unity editor).
            if (collider.CompareTag("Enemy"))
            {
                // Calculate the distance to the enemy.
                float distance = Vector3.Distance(transform.position, collider.transform.position);

                // If this enemy is closer than the current nearest enemy, update the nearest enemy.
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = collider.GetComponent<Enemy>();
                }
            }
        }

        // If a nearest enemy is found, apply damage.
        if (nearestEnemy != null)
        {
            // Determine the damage based on the enemy type.
            int damageToApply = GetDamageBasedOnEnemyType(nearestEnemy);

            // Apply damage to the nearest enemy.
            nearestEnemy.TakeDamage(damageToApply);
        }


    }
    // Coroutine to reset the attack state
    private IEnumerator ResetAttackState()
    {
        // Wait for the specified attackDuration
        yield return new WaitForSeconds(attackDuration);

        // Reset the "isAttacking" parameter
        animator.SetBool("isAttacking", false);
        // Reset the attack flag to allow the player to attack again.
        hasAttackedThisClick = false;
    }
   
    private int GetDamageBasedOnEnemyType(Enemy enemy)
    {
        /* Here, you can determine the damage based on the enemy's type.
        if (enemy is ZombieEnemy)
        {
            return zombieDamageAmount;
        }
        else if (enemy is GoblinEnemy)
        {
            return goblinDamageAmount;
        }
        else
        {
            return defaultDamageAmount;
        }
        */
        Debug.Log("hitted!!!");
        return defaultDamageAmount;
        
    }
    public void TakeDamage(int damage)
    {
        // Check if the player is currently invulnerable (during an attack animation).
        if (isInvulnerable)
        {
            // The player is invulnerable; no damage is taken.
            return;
        }

        // Reduce player's health based on the received damage.
        currentHealth -= damage;
        // Ensure that health doesn't go below zero.
        currentHealth = Mathf.Max(currentHealth, 0);
        // Log the current HP to the Unity console.
        Debug.Log("Current HP: " + currentHealth);
        HpBar hpBarController = GetComponentInChildren<HpBar>(true);

        Debug.Log(hpBarController.displayDuration);
        if (hpBarController != null)
        {
            hpBarController.UpdateHPBar(currentHealth, maxHealth);
        }

        // Handle player's death if health reaches zero.
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    // Add a method to set the "isBeingAttacked" flag when hit by an enemy.
    // Add a method to set the player's invulnerability status during attack animations.
    public void SetInvulnerable(bool invulnerable)
    {
        isInvulnerable = invulnerable;
    }
    private void Die()
    {
        // Implement player death behavior here (e.g., game over screen, respawn logic).
        // For now, we'll simply deactivate the player's GameObject.
        gameObject.SetActive(false);
    }

}
