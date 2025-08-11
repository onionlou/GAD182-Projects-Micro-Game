using UnityEngine;

public class DodgeWinCondition : MonoBehaviour, IWinCondition, IProjectileReactive
{
    [SerializeField] private float timeLimit = 15f;
    private float timer;
    private bool playerHit = false;
    private bool timeExpired = false;

    private void Start()
    {
        timer = timeLimit;
    }

    private void Update()
    {
        if (timeExpired || playerHit) return;

        timer -= Time.deltaTime;
        if (timer <= 0f) timeExpired = true;
    }

    // Called by projectile when it hits something
    public void OnProjectileHit(string hitTag)
    {
        if (hitTag == "Player")
        {
            playerHit = true;
        }
    }

    public bool CheckWinCondition()
    {
        return timeExpired && !playerHit;
    }

    public bool CheckLossCondition()
    {
        return playerHit;
    }
}
