using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Word typing game adjusted to use IWinCondition.
/// </summary>
public class Wordcheck : MonoBehaviour, IWinCondition
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
    public List<TMP_FontAsset> FontList = new List<TMP_FontAsset>();

    // IWinCondition events
    public event System.Action OnWin;
    public event System.Action OnLose;

    // --------------------------
    // OLD: Ended game locally with GameEnd coroutine
    // IEnumerator GameEnd(bool score)
    // {
    //     IsGameOver = true;
    //     TextField.SetActive(false);
    //     yield return new WaitForSeconds(3);
    // }
    // --------------------------

    // Start game
    void Start()
    {
        RoundStart();
    }

    // Start round
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

    // Check typed word
    public void CheckWord(string TypedWord)
    {
        if (IsGameOver) return;

        if (TypedWord == activeWord)
        {
            if (WonRounds == WonRoundToWinGame)
            {
                Debug.Log("Wordcheck: Won Game!");
                IsGameOver = true;
                OnWin?.Invoke(); // <-- NEW: trigger GameManager
                return;
            }

            WonRounds++;
            StartNewCoroutine(WaitForAnimation());
            Debug.Log("Wordcheck: Won round");
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
    }

    public void Lost()
    {
        if (IsGameOver) return;

        if (LostRounds >= lostRoundToFailGame)
        {
            Debug.Log("Wordcheck: Lost Game!");
            IsGameOver = true;
            OnLose?.Invoke(); // <-- NEW: trigger GameManager
            return;
        }

        LostRounds++;
        RoundStart();
        Debug.Log("Wordcheck: Lost round");
    }

    IEnumerator Stopwatch()
    {
        yield return new WaitForSeconds(TimeToAnswer);

        if (!IsGameOver)
        {
            Lost();
        }
    }

    // Utility: start coroutines safely
    public void StartNewCoroutine(IEnumerator routine)
    {
        if (activeCoroutine != null)
            StopCoroutine(activeCoroutine);

        activeCoroutine = StartCoroutine(routine);
    }

    // IWinCondition implementation
    public bool CheckWinCondition() => WonRounds >= WonRoundToWinGame && IsGameOver;
    public bool CheckLoseCondition() => LostRounds >= lostRoundToFailGame && IsGameOver;
}
