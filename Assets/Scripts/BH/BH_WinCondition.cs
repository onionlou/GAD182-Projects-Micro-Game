using UnityEngine;

public class BH_WinCondition : MonoBehaviour, IWinCondition
{
    [SerializeField] private float survivalTime = 10f; // win after 10s
    private float timer = 0f;

    private bool playerDead = false;
    private bool outcomeTriggered = false;

    public event System.Action OnWin;
    public event System.Action OnLose;

    private void Start()
    {
        var player = GameObject.Find("Player").GetComponent<BH_Player>();
        player.OnPlayerHit += HandlePlayerHit;
    }

    private void Update()
    {
        if (outcomeTriggered || playerDead) return;

        timer += Time.deltaTime;

        if (timer >= survivalTime)
        {
            TriggerWin();
        }
    }

    private void HandlePlayerHit()
    {
        playerDead = true;
        TriggerLose();
    }

    private void TriggerWin()
    {
        if (outcomeTriggered) return;
        outcomeTriggered = true;
        Debug.Log("BH_WinCondition: ✔ Player survived!");
        OnWin?.Invoke();
    }

    private void TriggerLose()
    {
        if (outcomeTriggered) return;
        outcomeTriggered = true;
        Debug.Log("BH_WinCondition: ✖ Player died!");
        OnLose?.Invoke();
    }

    public bool CheckWinCondition() => timer >= survivalTime && !playerDead;
    public bool CheckLoseCondition() => playerDead;
}
