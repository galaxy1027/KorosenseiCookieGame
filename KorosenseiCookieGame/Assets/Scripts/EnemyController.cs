using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [SerializeField] float speed = 1f;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float moveRange = 2f;
    int direction = 1;
    Vector3 spawnPoint;
    Vector3 currentPosition;
    void Start()
    {
        spawnPoint = transform.position;
    }
    void FixedUpdate()
    {
        ProcessMovement();
    }

    void ProcessMovement()
    {
        currentPosition = transform.position;

        if ((currentPosition.x < spawnPoint.x - moveRange) || (currentPosition.x > spawnPoint.x + moveRange)) // Flip direction if the enemy is outside of movement range
            direction *= -1;

        rb.linearVelocityX = speed * direction;
    }
}
