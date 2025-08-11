using UnityEngine;

public class CannonFodderWinCondition : MonoBehaviour, IWinCondition, IProjectileReactive
{
    [SerializeField] private float timeLimit = 15f;
    private float timer;
    private bool goblinHit = false;
    private bool timeExpired = false;

    private void Start()
    {
        timer = timeLimit;
    }

    private void Update()
    {
        if (timeExpired || goblinHit) return;

        timer -= Time.deltaTime;
        if (timer <= 0f) timeExpired = true;
    }

    // Called when a projectile hits something
    public void OnProjectileHit(string hitTag)
    {
        if (hitTag == "Enemy")
        {
            goblinHit = true;
        }
    }

    public bool CheckWinCondition()
    {
        return goblinHit;
    }

    public bool CheckLossCondition()
    {
        return timeExpired && !goblinHit;
    }
}
