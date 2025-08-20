using UnityEngine;
using System;
using System.Collections;

public class CannonFodderWinCondition : MonoBehaviour, IWinCondition
{
    public event Action OnWin;
    public event Action OnLose;

    [SerializeField] private float timeLimit = 5f;
    private float timer;
    private bool goblinHit = false;
    private bool sceneReady = false;

    private IEnumerator Start()
    {
        Debug.Log("CannonFodderWinCondition initialized.");
        yield return new WaitForSeconds(0.5f); // Allow scene to fully initialize
        timer = timeLimit;
        sceneReady = true;
    }

    private void Update()
    {
        if (!sceneReady || goblinHit) return;

        timer -= Time.deltaTime;
        if (timer <= 0f && CheckLoseCondition())
        {
            Debug.Log("✔ Lose condition met.");
            OnLose?.Invoke();
        }
    }

    public void OnProjectileHit(string hitTag)
    {
        if (!sceneReady || goblinHit) return;

        if (hitTag == "Enemy")
        {
            goblinHit = true;
            if (CheckWinCondition())
            {
                Debug.Log("✔ Win condition met.");
                OnWin?.Invoke();
            }
        }
    }

    // ✅ Interface implementations
    public bool CheckWinCondition()
    {
        return goblinHit;
    }

    public bool CheckLoseCondition()
    {
        return !goblinHit && timer <= 0f;
    }
}