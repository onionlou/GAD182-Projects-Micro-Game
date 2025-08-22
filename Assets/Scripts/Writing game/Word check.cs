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

    public int LostRounds;
    public int WonRounds;
    public int lostRoundToFailGame = 3;
    public int WonRoundToWinGame = 3;

    public float TimeToAnswer;
    bool IsGameOver;

    public GameObject TextField;
    public TMP_Text SpellText;

    private Coroutine activeCoroutine;

    public Animator HeroAni;
    public Animator EnemyAni;


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
        EnemyAni.SetFloat("Stage Float", LostRounds);

        StartNewCoroutine(Stopwatch());
        SpellText.rectTransform.anchoredPosition = new Vector3(336, -261, 0);

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
            if (WonRounds == WonRoundToWinGame)
            {
                //win end game

                //enemy dies
                Debug.Log("won Game");
                StartNewCoroutine(GameEnd(true));

                return;
            }


            //win round


            WonRounds++;
            StartNewCoroutine(WaitForAnimation());
            Debug.Log("won round");


        }
        else
        {
            Lost();
        }

    }
    IEnumerator WaitForAnimation()
    {
        SpellText.rectTransform.anchoredPosition = new Vector3(353, -403, 0);
        HeroAni.SetBool("Is Attacking", true);
        yield return new WaitForSeconds(1);
        HeroAni.SetBool("Is Attacking", false);

        RoundStart();
        yield return null;
    }

    public void Lost()
    {
        if (LostRounds >= lostRoundToFailGame)
        {
            //lose end game
            //you die
            Debug.Log("lost Game");

            StartNewCoroutine(GameEnd(false));
            return;
        }

        //lose round

        //shoot dud at enemy

        LostRounds++;
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

    IEnumerator GameEnd(bool score)
    {
        IsGameOver = true;
        TextField.SetActive(false);

        yield return new WaitForSeconds(3);

        //if score is true = won
        //if score is flase = lost

        yield return null;
    }


    //to add
    //  death animation
    //  count down on UI 


}
