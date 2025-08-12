using UnityEngine;

public class CannonFodderWinCondition : MonoBehaviour, IWinCondition, IProjectileReactive
{
    [SerializeField] private float timeLimit = 15f;

    private float timer;
    private bool goblinHit = false;

    public event System.Action OnWin;
    public event System.Action OnLose;

    private void Start()
    {
        timer = timeLimit;
    }

    private void Update()
    {
        // Decrease timer until time runs out or win condition is met
        if (!goblinHit)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f && CheckLoseCondition())
            {
                OnLose?.Invoke();
            }
        }
    }

    // Triggered when a projectile hits something
    public void OnProjectileHit(string hitTag)
    {
        if (hitTag == "Enemy" && !goblinHit)
        {
            goblinHit = true;
            if (CheckWinCondition())
            {
                OnWin?.Invoke();
            }
        }
    }

    public bool CheckWinCondition()
    {
        return goblinHit;
    }

    public bool CheckLoseCondition()
    {
        return !goblinHit && timer <= 0f;
    }
}
