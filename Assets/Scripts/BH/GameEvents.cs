using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake()
    {
        current = this;
    }

    public event Action OnPlayerHit;
    public void PlayerHit()
    {
       

        StartCoroutine("GameEnd");

        if (OnPlayerHit != null)
        {
            OnPlayerHit();
            
        }
    }
    IEnumerator GameEnd()
    {
       yield return new WaitForSeconds(4);

        //then Game end HERE
        Debug.Log("Game has ended");
       yield return null;
    }

}
