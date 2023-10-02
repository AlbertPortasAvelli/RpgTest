using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    protected int currentHealth;
    public Text floatingHPText; // Reference to the Text component.
    public float heightOffset = 2.0f; // Adjust this value to control the height offset of the HP text.
    public float textDisplayDuration = 2.0f; // Adjust this value to control how long the text is displayed.
    public float detectionRange = 10.0f; // Adjust this to set the enemy's detection range.
    private Vector3 initialPosition;
    private NavMeshAgent navMeshAgent;
    private Transform player;
    protected Animator animator;
    private Quaternion initialRotation;
    public float rotationSpeed = 5.0f; // Adjust this value to control the rotation speed.
    private float playerOutOfRangeTimer = 0f;
    public float attackRange = 5.0f;
    public int attackDamage = 10;
    public float attackAnimationDuration = 2.0f;
    protected bool isOnCooldown = false;
    public float attackCooldown = 3.0f; // Adjust this value as needed for the cooldown duration.
    public LayerMask playerLayer;




    protected virtual void Start()
    {
        initialRotation = transform.rotation;
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        // Initialize navMeshAgent.
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = 1.0f; // Adjust this to control how close the enemy gets to the player.

        // Find the player's GameObject (assuming the player has a "Player" tag).
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Store the initial position of the enemy.
        initialPosition = transform.position;
    }
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            // Player is within detection range; reset the timer.
            playerOutOfRangeTimer = 0f;
            animator.SetBool("isSleeping", false);
            // Check if the player is within attack range.
            if (distanceToPlayer <= attackRange)
            {
                // Perform the attack.
                Attack();
            }
            else
            {
                // Enemy is within detection range but not in attack range; make it follow the player.
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(player.position);
                animator.SetBool("isWalking", true);
            }


        }
        else
        {
            // Player is out of range; return the enemy to its initial position.
            navMeshAgent.isStopped = false; // Ensure the agent is active.
            navMeshAgent.SetDestination(initialPosition);
            // Check if the enemy is close to its destination.
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                // Enemy has reached its initial position; stop walking animation.
                animator.SetBool("isWalking", false);
                // Calculate the lerp factor based on rotationSpeed.
                float lerpFactor = Time.deltaTime * rotationSpeed;

                // Smoothly rotate back to the initial rotation.
                transform.rotation = Quaternion.Lerp(transform.rotation, initialRotation, lerpFactor);
                // Player is out of range; increment the timer.
                playerOutOfRangeTimer += Time.deltaTime;

                // Check if the timer exceeds a certain duration (e.g., 10 seconds) to put the dragon to sleep.
                if (playerOutOfRangeTimer >= 10f)
                {
                    // Start the sleep animation or set a boolean parameter for sleep state.
                    animator.SetBool("isSleeping", true);
                }
            }
            // Check if the player is within attack range while returning to initial position.
            if (distanceToPlayer <= attackRange)
            {
                // Perform the attack.
                Attack();
            }

        }
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Update the floating text label.
            UpdateFloatingHPText();
            // Start a coroutine to hide the text after a certain duration.
            StartCoroutine(HideFloatingHPText());
        }
    }
    public virtual void Attack()
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

        
        // Find the player within the attack range.
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange, playerLayer);

        // Check if a player is found within the attack range.
        if (hitColliders.Length > 0)
        {
            // Assuming there's only one player, directly access the first element.
            PlayerController player = hitColliders[0].GetComponent<PlayerController>();

            // Apply damage to the player.
            if (player != null)
            {
                player.TakeDamage(attackDamage);
                // Implement any attack-specific logic (e.g., dealing damage).
                Debug.Log("Enemy attacked for " + attackDamage + " damage.");
            }
        }
        // Set the enemy on cooldown.
        isOnCooldown = true;
        // Start the coroutine to reset the animation and set the cooldown.
        StartCoroutine(ResetAnimationAndCooldown());
    }

    protected virtual void Die()
    {
        // Implement the death behavior here.
        // For example, play death animations and destroy the GameObject.
        Destroy(gameObject);
    }
    private void UpdateFloatingHPText()
    {
       

            // Customize the text to display the enemy's current health.
            floatingHPText.text = "HP: " + currentHealth;
        
    }
    private IEnumerator HideFloatingHPText()
    {
        // Wait for the specified duration.
        yield return new WaitForSeconds(textDisplayDuration);

        // Hide the text by setting it to an empty string.
        floatingHPText.text = "";
    }
    protected IEnumerator ResetAnimationAndCooldown()
    {
        yield return new WaitForSeconds(attackAnimationDuration);

        // Transition back to the default state.
        animator.SetTrigger("ExitAttack"); // You might need to create an exit trigger.

        // Implement any additional logic needed after the attack animation.

        // Set the enemy on cooldown.
        isOnCooldown = true;
        yield return new WaitForSeconds(attackCooldown);

        // Reset the cooldown.
        isOnCooldown = false;
        // Find the player object.
        PlayerController playerController = player.GetComponent<PlayerController>();

        if (playerController != null)
        {
            // Reset the player's invulnerability status.
            playerController.SetInvulnerable(false);
        }
    }



}
