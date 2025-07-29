using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls game timer, win/lose state, and scene transitions.
/// Singleton pattern used.
/// </summary>
public class GameManager1 : MonoBehaviour
{
    public static GameManager1 Instance { get; private set; }

    [SerializeField] private float timeLimit = 10f;

    private float timer;
    private bool gameEnded = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        timer = timeLimit;
    }

    private void Update()
    {
        if (gameEnded) return;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Win();
        }
    }

    /// <summary>
    /// Call this when a fireball hits the player or goblin.
    /// </summary>
    public void GameOverOrWin(string hitTag)
    {
        if (gameEnded) return;

        if (hitTag == "Player")
        {
            Debug.Log("You Lose!");
            Debug.Log("Load 'Try Again' scene.");
            gameEnded = true;
        }
        else if (hitTag == "Goblin")
        {
            Win();
        }
    }

    private void Win()
    {
        Debug.Log("You Win!");
        Debug.Log("Load 'Next level' menu");
        gameEnded = true;
        Invoke(nameof(NextLevel), 2f); 
    }

    /// <summary>
    /// //Load scene for "2 Cannon Fodder"
    /// </summary>
    private void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}




