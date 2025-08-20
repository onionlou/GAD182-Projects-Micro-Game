using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMash : MonoBehaviour
{

    public int pressThreshold = 15;
    private int pressCount = 0;

    public GameObject[] prefabsToSwitch;
    private int currentPrefabIndex = 0;
    private GameObject currentInstance;
    public Transform spawnPoint;

    void Start()
    {
        SpawnPrefab(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pressCount++;
            Debug.Log("Pressed: " + pressCount);

            if (pressCount >= pressThreshold)
            {
                pressCount = 0;
                SwitchPrefab();
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
        int nextIndex = (currentPrefabIndex + 1) % prefabsToSwitch.Length;
        SpawnPrefab(nextIndex);
    }
}
