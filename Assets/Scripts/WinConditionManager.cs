using UnityEngine;
using System.Linq;

public class WinConditionManager : MonoBehaviour
{
    private IWinCondition[] winConditions;
    public bool gameEnded = false;

    void Start()
    {
        // Find all MonoBehaviours that implement IWinCondition
        winConditions = FindObjectsOfType<MonoBehaviour>(true)
            .OfType<IWinCondition>()
            .ToArray();
    }

    void Update()
    {
        if (gameEnded) return;

        bool allWin = true;
        bool anyLose = false;

        foreach (var condition in winConditions)
        {
            if (!condition.CheckWinCondition())
                allWin = false;

            if (condition.CheckLoseCondition())
                anyLose = true;
        }

        if (allWin)
            HandleGameOver(true);
        else if (anyLose)
            HandleGameOver(false);
    }

    private void HandleGameOver(bool win)
    {
        gameEnded = true;

        if (win)
        {
            Debug.Log("You win!");
            SceneSwapper.instance.LoadUnloadScene("Win Screen");
        }
        else
        {
            Debug.Log("You lose!");
            SceneSwapper.instance.LoadUnloadScene("Lose Screen");
        }
    }
}
