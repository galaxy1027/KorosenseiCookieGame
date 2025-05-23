using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int score { get; set; }

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddScore(int points)
    {
        score += points;
    }

}
