using UnityEngine;
using System.Collections;

public class PlayerJump : MonoBehaviour
{
    public float jumpForce = 8f;
    public float groundCheckDistance = 0.2f;
    public float minAirTimeBeforeJumpDown = 0.7f;

    private bool isJumpingUp = false;
    private bool isJumpingDown = false;
    private float jumpStartTime;
    private bool isGrounded = true;

    private Rigidbody rb;
    private Animator animator;
    public LayerMask groundLayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            if (Input.GetButtonDown("Jump"))
            {
                JumpUp();
            }
        }
        else
        {
            if (isJumpingUp && !isJumpingDown)
            {
                if (!isGrounded)
                {
                    if (Time.time - jumpStartTime >= minAirTimeBeforeJumpDown)
                    {
                        StartJumpDown();
                       FallGroundCheker();
                    }
                }
            }
        }
        
    }

    private void JumpUp()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isJumpingUp = true; // Set isJumpingUp to true when jump starts.
        isJumpingDown = false;
        jumpStartTime = Time.time;
        animator.SetBool("isJumpingUp", isJumpingUp);
        animator.SetBool("isJumpingDown", isJumpingDown);
    }
    public void OnJumpAnimationComplete()
    {
        FallGroundCheker();
    }

    private void FallGroundCheker()
    {
        // Set isJumpingUp to false when the player falls back to the ground.
        isJumpingUp = false;
        isJumpingDown = false;
        animator.SetBool("isJumpingUp", isJumpingUp);
        animator.SetBool("isJumpingDown", isJumpingDown);
    }


    private void StartJumpDown()
    {
        isJumpingUp = false;
        isJumpingDown = true;
        animator.SetBool("isJumpingUp", isJumpingUp);
        animator.SetBool("isJumpingDown", isJumpingDown);
    }

    public bool IsJumpingDown
    {
        get { return isJumpingDown; }
    }
    public bool IsJumpingUp
    {
        get { return isJumpingUp; }
    }
   

}
