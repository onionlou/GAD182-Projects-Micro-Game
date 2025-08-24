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
            Debug.LogError("WordcheckWinCondition: No Wordcheck script found in scene!");
            return;
        }

        Debug.Log("WordcheckWinCondition initialized.");
    }

    private void Update()
    {
        if (wordcheck == null || outcomeTriggered) return;

        if (CheckWinCondition())
        {
            TriggerWin();
        }
        else if (CheckLoseCondition())
        {
            TriggerLose();
        }
    }

    private void TriggerWin()
    {
        if (outcomeTriggered) return;
        outcomeTriggered = true;

        Debug.Log("WordcheckWinCondition: Player won.");
        OnWin?.Invoke();
    }

    private void TriggerLose()
    {
        if (outcomeTriggered) return;
        outcomeTriggered = true;

        Debug.Log("WordcheckWinCondition: Player lost.");
        OnLose?.Invoke();
    }

    // IWinCondition implementation
    public bool CheckWinCondition() =>
        wordcheck != null && wordcheck.WonRounds >= wordcheck.WonRoundToWinGame;

    public bool CheckLoseCondition() =>
        wordcheck != null && wordcheck.LostRounds >= wordcheck.lostRoundToFailGame;
}
