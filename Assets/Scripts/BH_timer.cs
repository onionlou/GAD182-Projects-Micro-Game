using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BH_timer : MonoBehaviour
{
    
    public float GameLength = 30;


    // Start is called before the first frame update
    
    private void Update()
    {
        GameLength -= Time.deltaTime;

        //while (GameLength > 0)
        {
            Debug.Log(GameLength);

        }


        if (GameLength < 0)
        {
            //end game

        }
    }





}
