using UnityEngine;

public class Cookie : MonoBehaviour
{
    [SerializeField] float hoverHeight;
    [SerializeField] float hoverFrequency;
    float height; // Where the object should move to on the y-axis
    float baseHeight; // The base y-coordinate of the object, where it should start before starting sine movement (center of the sine wave)

    void Start()
    {
        baseHeight = transform.position.y;
    }
    void Update()
    {
        height = baseHeight + Mathf.Sin(Time.time * hoverFrequency) * hoverHeight;
        transform.position = new Vector2(transform.position.x, height);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.AddScore(1);
            Destroy(gameObject);
        }
    }
}
