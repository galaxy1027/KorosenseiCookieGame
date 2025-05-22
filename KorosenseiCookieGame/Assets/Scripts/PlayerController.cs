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
    float velocity; // Player's current speed (and direction)
    float moveDirection; // Direction player is moving horizontally


    void Awake()
    {
        playerControls = new PlayerInputActions();
    }



    void OnEnable()
    {
        move = playerControls.Player.Move;
        jump = playerControls.Player.Jump;
        move.Enable();
        jump.Enable();
    }

    void OnDisable()
    {
        move.Disable();
        jump.Disable();
    }

    void Update()
    {
        ProcessMovement();
        ProcessJump();
    }

    void FixedUpdate()
    {

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
