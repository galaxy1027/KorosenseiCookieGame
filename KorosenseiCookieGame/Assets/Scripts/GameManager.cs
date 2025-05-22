using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score { get; set; }

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddScore(int points)
    {
        score += points;
    }

}
