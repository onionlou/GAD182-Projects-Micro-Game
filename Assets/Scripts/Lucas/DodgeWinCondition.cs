using UnityEngine;

public class DodgeWinCondition : MonoBehaviour, IWinCondition
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

        if (timer <= 0f)
        {
            timeExpired = true;
        }
    }

    public void RegisterHit(string hitTag)
    {
        if (hitTag == "Player")
        {
            playerHit = true;
        }
    }

    public bool CheckWin()
    {
        return timeExpired && !playerHit;
    }

    public bool CheckLoss()
    {
        return playerHit;
    }
}