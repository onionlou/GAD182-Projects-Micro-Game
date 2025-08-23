using UnityEngine;

public class WordcheckWinCondition : MonoBehaviour, IWinCondition
{
    private Wordcheck wordcheck;

    public event System.Action OnWin;
    public event System.Action OnLose;

    private bool outcomeTriggered = false;

    private void Start()
    {
        wordcheck = FindObjectOfType<Wordcheck>();

        if (wordcheck == null)
        {
            Debug.LogError("❌ No Wordcheck script found in scene!");
            return;
        }

        // Hook into Wordcheck's end logic
        StartCoroutine(GameEndWatcher());
        Debug.Log("✔ WordcheckWinCondition initialized.");
    }

    private System.Collections.IEnumerator GameEndWatcher()
    {
        // Poll for game end
        while (!wordcheckIsGameOver())
        {
            yield return null;
        }

        if (!outcomeTriggered)
        {
            if (wordcheck.WonRounds >= wordcheck.WonRoundToWinGame)
                TriggerWin();
            else if (wordcheck.LostRounds >= wordcheck.lostRoundToFailGame)
                TriggerLose();
        }
    }

    private void TriggerWin()
    {
        if (outcomeTriggered) return;
        outcomeTriggered = true;
        Debug.Log("✔ WordcheckWinCondition: Player won.");
        OnWin?.Invoke();
    }

    private void TriggerLose()
    {
        if (outcomeTriggered) return;
        outcomeTriggered = true;
        Debug.Log("✖ WordcheckWinCondition: Player lost.");
        OnLose?.Invoke();
    }

    public bool CheckWinCondition() =>
        wordcheck.WonRounds >= wordcheck.WonRoundToWinGame;

    public bool CheckLoseCondition() =>
        wordcheck.LostRounds >= wordcheck.lostRoundToFailGame;

    private bool wordcheckIsGameOver()
    {
        return wordcheck == null || wordcheck.WonRounds >= wordcheck.WonRoundToWinGame || wordcheck.LostRounds >= wordcheck.lostRoundToFailGame;
    }
}
