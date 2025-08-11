using UnityEngine;

public class DodgeWinCondition : MonoBehaviour, IWinCondition, IProjectileReactive
{
    [SerializeField] private float timeLimit = 15f;
    private float timer;
    private bool playerHit = false;
    private bool timeExpired = false;

    public event System.Action OnWin;
    public event System.Action OnLose;

    private void Start()
    {
        timer = timeLimit;
        Debug.Log("DodgeWinCondition initialized.");
    }

    private void Update()
    {
        if (timeExpired || playerHit) return;

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            timeExpired = true;
            if (CheckWinCondition())
            {
                Debug.Log("? Win condition met — time expired without player hit.");
                OnWin?.Invoke();
            }
        }

        if (CheckLoseCondition())
        {
            Debug.Log("? Lose condition met — player was hit.");
            OnLose?.Invoke();
        }
    }

    public void OnProjectileHit(string hitTag)
    {
        Debug.Log($"Projectile hit detected: {hitTag}");
        if (hitTag == "Player")
        {
            playerHit = true;
            if (CheckLoseCondition())
            {
                Debug.Log("? Triggering lose due to player hit.");
                OnLose?.Invoke();
            }
        }
    }

    public bool CheckWinCondition() => timeExpired && !playerHit;
    public bool CheckLoseCondition() => playerHit;
}