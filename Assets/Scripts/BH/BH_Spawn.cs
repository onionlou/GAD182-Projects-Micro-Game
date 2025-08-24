using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bullet Hell spawner that now integrates with IWinCondition instead of GameEvents.
/// The player wins if they survive for 'survivalTime' seconds.
/// </summary>
public class BH_Spawn : MonoBehaviour, IWinCondition
{
    public bool IsSpawning = true;
    public AnimationCurve SpawnCurve;
    public List<GameObject> ItemsToSpawn = new List<GameObject>();

    [Header("Win Condition Settings")]
    public float survivalTime = 10f; // <-- NEW: player must survive this long
    private float elapsedTime = 0f;
    private bool isGameOver = false;

    // IWinCondition events
    public event System.Action OnWin;
    public event System.Action OnLose;

    // --------------------------
    // OLD: Used GameEvents, now commented
    // IEnumerator Start()
    // {
    //     GameEvents.current.OnPlayerHit += PlayerHit;
    //     while (IsSpawning)
    //     {
    //         yield return new WaitForSeconds(SpawnCurve.Evaluate(Time.time));
    //         Spawner();
    //     }
    //     yield return null;
    // }
    // --------------------------

    private void Start()
    {
        // Start coroutine manually instead of relying on GameEvents
        StartCoroutine(SpawnLoop());
    }

    private void Update()
    {
        if (isGameOver) return;

        // Track survival time
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= survivalTime)
        {
            Debug.Log("BH_Spawn: Survived! Triggering OnWin.");
            isGameOver = true;
            IsSpawning = false;

            OnWin?.Invoke();
        }
    }

    private IEnumerator SpawnLoop()
    {
        while (IsSpawning)
        {
            yield return new WaitForSeconds(SpawnCurve.Evaluate(Time.time));
            Spawner();
        }
    }

    public void Spawner()
    {
        int RandomItemFromList = Random.Range(0, ItemsToSpawn.Count);
        float SpawnRange = Random.Range(-8f, 8f);
        Vector3 Location = new Vector3(SpawnRange, 5.67f, 0);
        Instantiate(ItemsToSpawn[RandomItemFromList], Location, Quaternion.identity);
    }

    // --------------------------
    // OLD: Called via GameEvents when player was hit
    // public void PlayerHit()
    // {
    //     IsSpawning = false;
    // }
    // --------------------------

    // IWinCondition implementation
    public bool CheckWinCondition() => elapsedTime >= survivalTime && !isGameOver;
    public bool CheckLoseCondition() => isGameOver && !CheckWinCondition();

    // Public method to trigger lose (called by BH_Player/BH_Item when player dies)
    public void TriggerLose()
    {
        

        if (isGameOver) return;
        Debug.Log("BH_Spawn: Player hit, triggering OnLose.");
        isGameOver = true;
        IsSpawning = false;
        OnLose?.Invoke();
    }
}
