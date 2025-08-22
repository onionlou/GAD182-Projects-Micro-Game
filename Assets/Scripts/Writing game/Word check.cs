using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Wordcheck : MonoBehaviour
{


    public List<string> words = new List<string>();
    int intWord;
    string activeWord;

    public int currentRound;
    public int MaxRounds = 3;

    public float TimeToAnswer;
    bool IsGameOver;

    public GameObject TextField;
    public TMP_Text SpellText;

    private Coroutine activeCoroutine;


    public List<TMPro.TMP_FontAsset> FontList = new List<TMPro.TMP_FontAsset>();


    //culls old coroutine, so they dont overlap into next round
    public void StartNewCoroutine(IEnumerator routine)
    {
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine); 
        }

        activeCoroutine = StartCoroutine(routine);
    }




    void Start()
    {
        RoundStart();
        
    }

    void RoundStart()
    {
        //enemy gets closer
        StartNewCoroutine(Stopwatch());

        intWord = Random.Range(0, words.Count);
        activeWord = words[intWord];

        SpellText.text = activeWord;

        int ActiveFont = Random.Range(0, FontList.Count);
        SpellText.font = FontList[ActiveFont];


    }




    public void CheckWord(string TypedWord)
    {
        if (TypedWord == activeWord)
        {
            if (currentRound >= MaxRounds)
            {
                //win end game

                //enemy dies
                Debug.Log("won Game");
                GameEnd(true);
                return;
            }


            //win round

            //shoot fireball at enemys
            currentRound++;
            RoundStart();
            Debug.Log("won round");


        }
        else
        {
            Lost();
        }

    }


    public void Lost()
    {
          if (currentRound >= MaxRounds)
        {
            //lose end game
            //you die
            Debug.Log("lost Game");
            
            GameEnd(false);
            return;
          }

            //lose round

            //shoot dud at enemy
            
            currentRound++;
            RoundStart();
            Debug.Log("lost round");

        
    }
   

    IEnumerator Stopwatch()
    {
        yield return new WaitForSeconds(TimeToAnswer);

        if (!IsGameOver)
        {
            Lost();
        }
            
        
            yield return null;

    }

    public void GameEnd(bool score)
    {
        IsGameOver = true;
        TextField.SetActive(false);

        //if score is true = won
        //if score is flase = lost

    }

}
