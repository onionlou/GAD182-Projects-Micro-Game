using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Wordcheck : MonoBehaviour
{


    public List<string> words = new List<string>();
    int intWord;
    string activeWord;

    public int currentRound;
    public int MaxRounds = 3;

    // Start is called before the first frame update
    void Start()
    {
        RoundStart();
    }

    void RoundStart()
    {
        //enemy gets closer

        intWord = Random.Range(0, words.Count);
        activeWord = words[intWord];

    }


    public void CheckWord(string TypedWord)
    {
        if (TypedWord == activeWord)
        {
            if (currentRound == MaxRounds)
            {
                //win end game

                //enemy dies
                return;
            }


            //win round

            //shoot fireball at enemys
            currentRound ++;
            Debug.Log("win");


        }
        else
        {
            if (currentRound == MaxRounds)
            {
                //lose end game
                //you die
                return;
            }

            //lose round

            //shoot dud at enemy
            currentRound++;
            
            Debug.Log("lose");
        }


    }
}
