using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed; // Player's maximum speed
    [SerializeField] float jumpForce; // How much force can be applied when jumping
    [SerializeField] PlayerInputActions playerControls;
    [SerializeField] Rigidbody2D rb;

    [SerializeField] Vector2 groundBoxSize;
    [SerializeField] float groundCastDistance;
    [SerializeField] LayerMask groundLayer;
    InputAction move;
    InputAction jump;
    InputAction crouch;
    float velocity; // Player's current speed (and direction)
    float moveDirection; // Direction player is moving horizontally
    float baseGravityScale; // How strong gravity should be normally, can change when fast falling
    [SerializeField] float maxFallSpeed; // Prevent the player from falling at insane speeds by clamping fall speed to this


    void Awake()
    {
        playerControls = new PlayerInputActions();
        baseGravityScale = rb.gravityScale;
    }

    void OnEnable()
    {
        move = playerControls.Player.Move;
        jump = playerControls.Player.Jump;
        crouch = playerControls.Player.Crouch;
        move.Enable();
        jump.Enable();
        crouch.Enable();
    }

    void OnDisable()
    {
        move.Disable();
        jump.Disable();
        crouch.Disable();
    }

    void Update()
    {
        ProcessMovement();
        ProcessJump();
    }

    void FixedUpdate()
    {
        /*
            Handling fall speed
            The player's velocity increases as they fall to make the jump feel less 'floaty'. This effect is strengthened if the player is pressing down.
         */
        if (rb.linearVelocityY < 0) // The player is moving downwards
        {
            if (crouch.IsPressed()) // The player wants to fast fall
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


        else if (!jump.IsPressed())
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
        moveDirection = move.ReadValue<float>();
        rb.linearVelocityX = moveDirection * moveSpeed;
    }

    void ProcessJump()
    {
        if (jump.triggered && IsGrounded())
        {
            rb.linearVelocityY += jumpForce;
        }
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


}
