using System;
using System.Collections;
using System.Collections.Generic;
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
        /// WAIT 5 SEC, then Game end HERE


        if (OnPlayerHit != null)
        {
            OnPlayerHit();
            
        }
    }
    

}
