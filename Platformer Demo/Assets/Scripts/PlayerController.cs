using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float maxSpeed = 5.0f; // Player's maximum speed
    [SerializeField] InputAction inputActions;
    float velocity; // Player's current speed (and direction)
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inputActions.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessMovement();
    }

    void ProcessMovement()
    {

    }
}
