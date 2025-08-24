using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CannonFodderWinCondition : MonoBehaviour, IProjectileReactive
{
    [SerializeField] private float timeLimit = 5f;
    [SerializeField] private string sceneTag = "CannonFodder";

    private float timer;
    private bool goblinHit = false;
    private bool sceneReady = false;
    private bool winTriggered = false;

    private IEnumerator Start()
    {
        Debug.Log($"[{sceneTag}] Initializing win condition...");
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
        if (timer <= 0f)
        {
            Debug.Log($"[{sceneTag}] ⏱ Time expired. No goblin hit.");
            winTriggered = true;
            HandleLose();
        }
    }

    public void OnProjectileHit(string hitTag)
    {
        Debug.Log($"[{sceneTag}] Projectile hit detected: {hitTag}");

        if (!sceneReady || winTriggered)
        {
            Debug.LogWarning($"[{sceneTag}] Hit ignored — scene not ready or win already triggered.");
            return;
        }

        if (hitTag == "Enemy")
        {
            Debug.Log($"[{sceneTag}] ✔ Goblin hit confirmed.");
            goblinHit = true;
            winTriggered = true;
            HandleWin();
        }
        else
        {
            Debug.Log($"[{sceneTag}] Hit ignored — tag '{hitTag}' not valid.");
        }
    }

    private void HandleWin()
    {
        Debug.Log($"[{sceneTag}] 🎉 Win condition met. Loading FINAL Win Menu...");
        Time.timeScale = 1f;
        SceneManager.LoadScene("FINAL Win Menu", LoadSceneMode.Single);
    }

    private void HandleLose()
    {
        Debug.Log($"[{sceneTag}] ❌ Lose condition met. No scene transition triggered.");
        // Optional: Load a lose scene or show UI here if needed
    }
}