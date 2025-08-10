using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerDodge : MonoBehaviour
{
    [SerializeField] private float timeLimit = 15f;
    [SerializeField] private MonoBehaviour winConditionComponent;

    private IWinCondition winCondition;
    private float timer;
    private bool gameEnded = false;

    private void Start()
    {
        winCondition = winConditionComponent as IWinCondition;
        timer = timeLimit;
    }

    private void Update()
    {
        if (gameEnded) return;

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            EvaluateOutcome();
        }
    }

    private void EvaluateOutcome()
    {
        if (winCondition.CheckWin())
        {
            Win();
        }
        else if (winCondition.CheckLoss())
        {
            Lose(); 
        }
    }


    private void Win()
    {
        gameEnded = true;
        Debug.Log("You dodged successfully! Loading next scene...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void Lose()
    {
        gameEnded = true;
        Debug.Log("You got hit! Reloading scene...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}