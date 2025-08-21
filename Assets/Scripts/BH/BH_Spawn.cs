using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BH_Spawn : MonoBehaviour
{
    public bool IsSpawning = true;
    public AnimationCurve SpawnCurve;

    public List<GameObject> ItemsToSpawn = new List<GameObject>();


    // Start is called before the first frame update
    
    private void Update()
    {



    }


    public void Spawner()
    {
        
        int RandomItemFromList = Random.Range(0, ItemsToSpawn.Count);

        float SpawnRange = Random.Range(-9.33f, 9.33f);
        Vector3 Location = new Vector3 (SpawnRange, 5.67f,0);
        Instantiate(ItemsToSpawn[RandomItemFromList],Location, Quaternion.identity);

    }


    IEnumerator Start()
    {
        GameEvents.current.OnPlayerHit += PlayerHit;

        while (IsSpawning)
        {


            yield return new WaitForSeconds(SpawnCurve.Evaluate(Time.time));
            Spawner();

        }
        yield return null;

    }
    public void PlayerHit()
    {
        IsSpawning = false;
    }


}
