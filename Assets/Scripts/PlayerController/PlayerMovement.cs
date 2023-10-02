using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 120f;

    private Rigidbody rb;
    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = transform.forward * moveVertical * speed;

        float rotationInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * rotationInput * rotationSpeed * Time.deltaTime);

        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        bool isWalking = Mathf.Abs(moveVertical) > 0.1f;
        animator.SetBool("isRunning", isWalking);
    }
}
