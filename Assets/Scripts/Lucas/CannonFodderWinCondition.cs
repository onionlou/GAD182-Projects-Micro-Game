using UnityEngine;

public class CannonFodderWinCondition : MonoBehaviour, IWinCondition
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

        if (timer <= 0f)
        {
            timeExpired = true;
        }
    }

    public void RegisterHit(string hitTag)
    {
        if (hitTag == "Enemy")
        {
            goblinHit = true;
        }
    }

    public bool CheckWin()
    {
        return goblinHit;
    }

    public bool CheckLoss()
    {
        return timeExpired && !goblinHit;
    }
}