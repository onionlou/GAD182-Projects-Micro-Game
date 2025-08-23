using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BH_Spawn : MonoBehaviour
{
    [Header("Spawning Settings")]
    public bool isSpawning = true;
    public AnimationCurve spawnCurve = AnimationCurve.Linear(0, 1f, 10, 0.2f);
    public List<GameObject> bulletPrefabs = new List<GameObject>();

    [Header("Spawn Range")]
    public float xMin = -9.33f;
    public float xMax = 9.33f;
    public float ySpawn = 5.67f;

    private BH_WinCondition winCondition;

    private void Start()
    {
        winCondition = FindObjectOfType<BH_WinCondition>();
        if (winCondition == null)
        {
            Debug.LogWarning("[BH_Spawn] No BH_WinCondition found. Spawning will not stop on lose.");
        }

        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (isSpawning)
        {
            if (winCondition != null && winCondition.CheckLoseCondition())
            {
                Debug.Log("[BH_Spawn] Player lost. Stopping spawn.");
                isSpawning = false;
                yield break;
            }

            float waitTime = spawnCurve.Evaluate(Time.time);
            yield return new WaitForSeconds(waitTime);

            SpawnBullet();
        }
    }

    private void SpawnBullet()
    {
        if (bulletPrefabs.Count == 0)
        {
            Debug.LogWarning("[BH_Spawn] No bullet prefabs assigned.");
            return;
        }

        int index = Random.Range(0, bulletPrefabs.Count);
        float xPos = Random.Range(xMin, xMax);
        Vector3 spawnPos = new Vector3(xPos, ySpawn, 0f);

        Instantiate(bulletPrefabs[index], spawnPos, Quaternion.identity);
    }
}