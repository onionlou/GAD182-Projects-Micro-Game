using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BH_Spawn : MonoBehaviour
{
    public bool FakeArgument = true;


    public List<GameObject> ItemsToSpawn = new List<GameObject>();


    // Start is called before the first frame update
    
    private void Update()
    {
        

        if (Input.GetKey(KeyCode.Space))
        {
            Spawner();

        }
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
        Debug.Log("IEnumerable Called");

        while (FakeArgument)
        {
            Debug.Log("Spawned");
            yield return new WaitForSeconds(0.5f);
            Spawner();

        }
        yield return null;

    }


}
