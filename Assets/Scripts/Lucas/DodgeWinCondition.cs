using UnityEngine;

public class DodgeWinCondition : MonoBehaviour, IWinCondition, IProjectileReactive
{
    [SerializeField] private float timeLimit = 15f;
    private float timer;
    private bool playerHit = false;
    private bool outcomeTriggered = false;

    public event System.Action OnWin;
    public event System.Action OnLose;

    private void Start()
    {
        timer = timeLimit;
        Debug.Log("DodgeWinCondition initialized.");
    }

    private void Update()
    {
        if (outcomeTriggered) return;

        // Countdown
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            // Only win if timer ran out and player was not hit
            if (!playerHit)
            {
                TriggerWin();
            }
        }
    }

    public void OnProjectileHit(string hitTag)
    {
        if (outcomeTriggered) return;

        Debug.Log($"Projectile hit detected: {hitTag}");
        if (hitTag == "Player")
        {
            playerHit = true;
            TriggerLose();
        }
    }

    private void TriggerWin()
    {
        if (outcomeTriggered) return;
        outcomeTriggered = true;
        Debug.Log("✔ Win condition met.");
        OnWin?.Invoke();
    }

    private void TriggerLose()
    {
        if (outcomeTriggered) return;
        outcomeTriggered = true;
        Debug.Log("✖ Lose condition met.");
        OnLose?.Invoke();
    }

    // These methods are optional but satisfy the interface
    public bool CheckWinCondition() => timer <= 0f && !playerHit;
    public bool CheckLoseCondition() => playerHit;
}
