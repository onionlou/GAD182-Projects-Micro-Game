using UnityEngine;
using System;

public class BH_WinCondition : MonoBehaviour, IWinCondition
{
    public event Action OnWin;
    public event Action OnLose;

    [SerializeField] private int hitsToWin = 10;

    private int hitsTaken = 0;
    private bool gameEnded = false;

    public void PlayerHit()
    {
        if (gameEnded)
        {
            return;
        }

        hitsTaken++;
        Debug.Log($"[BH_WinCondition] Player hit registered. Total hits: {hitsTaken}");

        if (hitsTaken >= hitsToWin)
        {
            Debug.Log("[BH_WinCondition] Hit threshold reached. Triggering win.");
            TriggerWin();
        }
    }

    public void TriggerWin()
    {
        if (gameEnded)
        {
            return;
        }

        gameEnded = true;
        Debug.Log("[BH_WinCondition] WIN triggered. Firing OnWin event.");
        OnWin?.Invoke();
    }

    public void TriggerLose()
    {
        if (gameEnded)
        {
            return;
        }

        gameEnded = true;
        Debug.Log("[BH_WinCondition] LOSE triggered. Firing OnLose event.");
        OnLose?.Invoke();
    }


    public bool CheckWinCondition() => !gameEnded && hitsTaken >= hitsToWin;
    public bool CheckLoseCondition() => gameEnded;
}