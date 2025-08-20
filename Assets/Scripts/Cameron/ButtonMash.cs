using UnityEngine;
using System;

public class ButtonMash : MonoBehaviour, IWinCondition
{
    public event Action OnWin;
    public event Action OnLose;

    [Header("Button Mash Settings")]
    public int pressThreshold = 15;
    private int pressCount = 0;

    [Header("Prefab Swapping")]
    public GameObject[] prefabsToSwitch;
    private int currentPrefabIndex = 0;
    private GameObject currentInstance;
    public Transform spawnPoint;

    [Header("Timer")]
    public float timeLimit = 12f;
    private float timer;
    private bool gameActive = true;
    private bool hasWon = false;
    private bool hasLost = false;

    void Start()
    {
        timer = timeLimit;
        SpawnPrefab(0);
    }

    void Update()
    {
        if (!gameActive) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            hasLost = true;
            gameActive = false;
            OnLose?.Invoke();
            Debug.Log("ButtonMash: Time ran out — Lose triggered.");
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            pressCount++;
            Debug.Log("Pressed: " + pressCount);

            if (pressCount >= pressThreshold)
            {
                pressCount = 0;
                SwitchPrefab();

                if (currentPrefabIndex >= 2) // Third prefab = win
                {
                    hasWon = true;
                    gameActive = false;
                    OnWin?.Invoke();
                    Debug.Log("ButtonMash: Win triggered!");
                }
            }
        }
    }

    void SpawnPrefab(int index)
    {
        if (currentInstance != null)
            Destroy(currentInstance);

        currentPrefabIndex = index % prefabsToSwitch.Length;
        currentInstance = Instantiate(prefabsToSwitch[currentPrefabIndex], spawnPoint.position, Quaternion.identity);
    }

    void SwitchPrefab()
    {
        int nextIndex = currentPrefabIndex + 1;
        SpawnPrefab(nextIndex);
    }

    public void ResetGame()
    {
        pressCount = 0;
        timer = timeLimit;
        hasWon = false;
        hasLost = false;
        gameActive = true;
        SpawnPrefab(0);
    }

    public bool CheckWinCondition() => hasWon;
    public bool CheckLoseCondition() => hasLost;
}