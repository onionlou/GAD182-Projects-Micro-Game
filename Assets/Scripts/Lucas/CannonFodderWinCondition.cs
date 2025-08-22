using UnityEngine;
using System;
using System.Collections;

public class CannonFodderWinCondition : MonoBehaviour, IWinCondition, IProjectileReactive
{
    public event Action OnWin;
    public event Action OnLose;

    [SerializeField] private float timeLimit = 5f;
    [SerializeField] private string sceneTag = "CannonFodder";

    private float timer;
    private bool goblinHit = false;
    private bool sceneReady = false;
    private bool winTriggered = false;

    private IEnumerator Start()
    {
        Debug.Log($"[{sceneTag}] WinCondition initializing...");

        goblinHit = false;
        winTriggered = false;
        sceneReady = false;
        timer = timeLimit;

        yield return new WaitForSeconds(0.5f); // Let scene objects settle

        sceneReady = true;
        Debug.Log($"[{sceneTag}] Scene ready.");
    }

    private void Update()
    {
        if (!sceneReady || goblinHit || winTriggered) return;

        timer -= Time.deltaTime;
        if (timer <= 0f && CheckLoseCondition())
        {
            Debug.Log($"[{sceneTag}] ✔ Lose condition met.");
            winTriggered = true;
            OnLose?.Invoke();
        }
    }

    public void OnProjectileHit(string hitTag)
    {
        Debug.Log($"[{sceneTag}] OnProjectileHit called with tag: {hitTag}");

        if (!sceneReady)
        {
            Debug.LogWarning($"[{sceneTag}] Hit ignored — scene not ready.");
            return;
        }

        if (winTriggered)
        {
            Debug.LogWarning($"[{sceneTag}] Hit ignored — win already triggered.");
            return;
        }

        if (hitTag == "Enemy")
        {
            Debug.Log($"[{sceneTag}] Valid hit registered.");
            goblinHit = true;

            if (CheckWinCondition())
            {
                Debug.Log($"[{sceneTag}] ✔ Win condition met.");
                winTriggered = true;
                OnWin?.Invoke();
            }
        }
        else
        {
            Debug.Log($"[{sceneTag}] Hit ignored — tag '{hitTag}' not valid.");
        }
    }

    public bool CheckWinCondition() => goblinHit;

    public bool CheckLoseCondition() => !goblinHit && timer <= 0f;
}