using Unity.VisualScripting;
using UnityEngine;

public class Cookie : MonoBehaviour
{
    [SerializeField] float hoverHeight;
    [SerializeField] float hoverFrequency;
    float height;
    float baseHeight; // The base y-coordinate of the object, where it should start before starting sine movement

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
            GameManager.Instance.AddScore(1);
            Destroy(gameObject);
        }
    }
}
