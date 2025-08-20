using UnityEngine;

public class DodgeWinCondition : MonoBehaviour, IWinCondition, IProjectileReactive
{
    [SerializeField] private float timeLimit = 15f;
    private float timer;
    private bool playerHit = false;
    private bool timeExpired = false;
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

        if (!timeExpired)
        {
            timer -= Time.deltaTime;

            if (timer <= 0f)
            {
                timeExpired = true;

                if (CheckWinCondition())
                {
                    outcomeTriggered = true;
                    Debug.Log("✔ Win condition met.");
                    OnWin?.Invoke();
                }
            }
        }

        if (CheckLoseCondition())
        {
            outcomeTriggered = true;
            Debug.Log("✖ Lose condition met.");
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