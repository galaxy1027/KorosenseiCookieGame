using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed; // Player's maximum speed
    [SerializeField] float jumpForce; // How much force can be applied when jumping
    // [SerializeField] PlayerInputActions playerControls;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Vector2 groundBoxSize; // Size of box used for box-cast grounding check
    [SerializeField] float groundCastDistance; // How far the box is from the player's origin
    [SerializeField] LayerMask groundLayer; // Reference to the ground layer
    [SerializeField] GameObject spawnPoint;
    float velocity; // Player's current speed (and direction)
    float moveDirection; // Direction player is moving horizontally
    float baseGravityScale; // How strong gravity should be normally, can change when fast falling
    [SerializeField] float maxFallSpeed; // Prevent the player from falling at insane speeds by clamping fall speed to this
    InputManager inputs; // Reference to the input manager

    void Start()
    {
        baseGravityScale = rb.gravityScale;
        inputs = InputManager.instance;
        transform.position = spawnPoint.transform.position;
    }

    void Update()
    {
        ProcessMovement();
        ProcessJump();
    }

    void Respawn()
    {
        transform.position = spawnPoint.transform.position;
    }

    void FixedUpdate()
    {
        /*
            Handling fall speed
            The player's velocity increases as they fall to make the jump feel less 'floaty'. This effect is strengthened if the player is pressing down.
         */
        if (rb.linearVelocityY < 0) // The player is moving downwards
        {
            if (inputs.crouch.IsPressed()) // The player wants to fast fall
            {
                Debug.Log("Fast falling");
                rb.gravityScale = baseGravityScale * 3f;
            }
            else
            {
                rb.gravityScale = baseGravityScale * 1.5f;
            }
            // Check to see if the player is moving too fast, restrict to maximum speed if needed.
            rb.linearVelocityX = Mathf.Max(rb.linearVelocityX, -maxFallSpeed);
        }


        else if (!inputs.jump.IsPressed())
        {
            rb.gravityScale = baseGravityScale * 3f;
        }

        else // Restore gravity to normal
        {
            rb.gravityScale = baseGravityScale;
        }
    }

    void ProcessMovement()
    {
        moveDirection = inputs.move.ReadValue<float>();
        rb.linearVelocityX = moveDirection * moveSpeed;
    }

    void ProcessJump()
    {
        if (inputs.jump.triggered && IsGrounded())
            rb.linearVelocityY += jumpForce;
    }

    /*
        Is Grounded: Checks if the player is touching the ground through a box cast
        Returns true if the player is touching the ground
    */
    bool IsGrounded()
    {
        return Physics2D.BoxCast(transform.position, groundBoxSize, 0, -transform.up, groundCastDistance, groundLayer);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * groundCastDistance, groundBoxSize);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            Respawn();
        }
    }

}
