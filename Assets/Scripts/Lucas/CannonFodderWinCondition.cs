using UnityEngine;

public class CannonFodderWinCondition : MonoBehaviour, IWinCondition, IProjectileReactive
{
    [SerializeField] private float timeLimit = 15f;
    private float timer;
    private bool goblinHit = false;
    private bool timeExpired = false;
    public event System.Action OnWin;
    public event System.Action OnLose;

    private void Start()
    {
        timer = timeLimit;
    }

    private void Update()
    {
        if (timeExpired || goblinHit) return;

        if (timer <= 0f)
        {
            timeExpired = true;
            if (CheckLoseCondition()) OnLose?.Invoke();
        }

    }

    // Called when a projectile hits something
    public void OnProjectileHit(string hitTag)
    {
        if (hitTag == "Enemy")
        {
            goblinHit = true;
            if (CheckWinCondition()) OnLose?.Invoke();
        }
    }

    public bool CheckWinCondition()
    {
        return goblinHit;
    }

    public bool CheckLoseCondition()
    {
        return timeExpired && !goblinHit;
    }
}
