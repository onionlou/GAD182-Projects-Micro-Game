using System.Collections;
using UnityEngine;
using TMPro;
using System;

public class RPGHandler : MonoBehaviour, IWinCondition
{
    public event Action OnWin;
    public event Action OnLose;

    public TextMeshProUGUI BattleText;
    public TextMeshProUGUI HPText;
    public TextMeshProUGUI GoblinText;
    public GameObject S_Goblin_256x256;

    public static string[] Situation = { "Fight", "Heal", "Run" };
    private string chosenSituation;
    private bool Pressed;
    private bool OutofTime;
    private bool winOrLoseTriggered;

    public float decisionTime;

    public AudioSource audioSource;
    public AudioClip music;
    public AudioClip hit;
    public AudioClip heal;
    public AudioClip hurt;
    public AudioClip run;

    void Start()
    {
        int randomIndex = UnityEngine.Random.Range(0, Situation.Length);
        chosenSituation = Situation[randomIndex];
        Debug.Log("[RPG] Situation: " + chosenSituation);

        decisionTime = 5.0f;
        audioSource.PlayOneShot(music);
        Pressed = false;
        OutofTime = false;
        winOrLoseTriggered = false;

        BattleText.text = "A goblin blocks your path. What do you do?\nLeft Arrow Key - Fight                         Right Arrow Key - Heal\n                             Down Arrow Key - Run";

        switch (chosenSituation)
        {
            case "Fight": Fight(); break;
            case "Heal": Heal(); break;
            case "Run": Run(); break;
            default: Debug.LogError("[RPG] Invalid situation."); break;
        }
    }

    void Update()
    {
        if (winOrLoseTriggered) return;

        decisionTime -= Time.deltaTime;

        if (decisionTime <= 0f && !Pressed && !OutofTime)
        {
            Debug.Log("[RPG] TIME UP");
            OutofTime = true;
            StartCoroutine(TimeUp());
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && !Pressed && decisionTime > 0f)
        {
            Pressed = true;
            BattleText.text = "You Attack the Goblin.";

            if (chosenSituation == "Fight") StartCoroutine(FightCorrect());
            else if (chosenSituation == "Heal") StartCoroutine(FightWrongH());
            else StartCoroutine(FightWrongR());
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && !Pressed && decisionTime > 0f)
        {
            Pressed = true;
            BattleText.text = "You healed all your HP.";
            HPText.text = "HP: 20";
            HPText.color = Color.white;
            audioSource.PlayOneShot(heal);

            if (chosenSituation == "Heal") StartCoroutine(HealCorrect());
            else if (chosenSituation == "Fight") StartCoroutine(HealWrongF());
            else StartCoroutine(HealWrongR());
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && !Pressed && decisionTime > 0f)
        {
            Pressed = true;
            BattleText.text = "You tried to run away.";

            if (chosenSituation == "Run") StartCoroutine(RunCorrect());
            else if (chosenSituation == "Fight") StartCoroutine(RunWrongF());
            else StartCoroutine(RunWrongH());
        }
    }

    void Fight() { HPText.text = "HP: 20"; GoblinText.color = Color.red; }
    void Heal() { HPText.text = "HP: 5"; HPText.color = Color.yellow; GoblinText.color = Color.yellow; }
    void Run() { HPText.text = "HP: 1"; HPText.color = Color.red; }

    IEnumerator FightCorrect()
    {
        yield return new WaitForSeconds(2f);
        BattleText.text = "The Goblin was defeated.";
        audioSource.PlayOneShot(hit);
        S_Goblin_256x256.SetActive(false);
        GoblinText.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        BattleText.text = "You Won!!!";
        TriggerWin();
    }

    IEnumerator FightWrongH()
    {
        yield return new WaitForSeconds(2f);
        BattleText.text = "You Missed!";
        yield return new WaitForSeconds(2f);
        HPText.text = "HP: 0";
        HPText.color = Color.red;
        BattleText.text = "You lost all your HP due to poison.";
        audioSource.PlayOneShot(hurt);
        yield return new WaitForSeconds(2f);
        BattleText.text = "Game Over.";
        TriggerLose();
    }

    IEnumerator FightWrongR()
    {
        yield return new WaitForSeconds(2f);
        BattleText.text = "The Goblin Lost 3HP";
        audioSource.PlayOneShot(hit);
        yield return new WaitForSeconds(2f);
        BattleText.text = "The Goblin Attacks!";
        yield return new WaitForSeconds(2f);
        HPText.text = "HP: 0";
        HPText.color = Color.red;
        BattleText.text = "You lost all your HP.";
        audioSource.PlayOneShot(hurt);
        yield return new WaitForSeconds(2f);
        BattleText.text = "Game Over.";
        TriggerLose();
    }

    IEnumerator HealCorrect()
    {
        yield return new WaitForSeconds(2f);
        BattleText.text = "The Goblin died due to poison.";
        audioSource.PlayOneShot(hit);
        S_Goblin_256x256.SetActive(false);
        GoblinText.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        BattleText.text = "You Won!!!";
        TriggerWin();
    }

    IEnumerator HealWrongF()
    {
        yield return new WaitForSeconds(2f);
        BattleText.text = "The Goblin Attacks!";
        yield return new WaitForSeconds(2f);
        HPText.text = "HP: 0";
        HPText.color = Color.red;
        BattleText.text = "You lost all your HP.";
        audioSource.PlayOneShot(hurt);
        yield return new WaitForSeconds(2f);
        BattleText.text = "Game Over.";
        TriggerLose();
    }

    IEnumerator HealWrongR()
    {
        yield return new WaitForSeconds(2f);
        BattleText.text = "The Goblin Attacks!";
        yield return new WaitForSeconds(2f);
        HPText.text = "HP: 0";
        HPText.color = Color.red;
        BattleText.text = "You lost all your HP.";
        audioSource.PlayOneShot(hurt);
        yield return new WaitForSeconds(2f);
        BattleText.text = "Game Over.";
        TriggerLose();
    }

    IEnumerator RunCorrect()
    {
        yield return new WaitForSeconds(2f);
        BattleText.text = "You escaped safely!";
        audioSource.PlayOneShot(run);
        S_Goblin_256x256.SetActive(false);
        GoblinText.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        BattleText.text = "You Won!!!";
        TriggerWin();
    }

    IEnumerator RunWrongF()
    {
        yield return new WaitForSeconds(2f);
        BattleText.text = "You tripped!";
        yield return new WaitForSeconds(2f);
        HPText.text = "HP: 0";
        HPText.color = Color.red;
        BattleText.text = "The Goblin caught you.";
        audioSource.PlayOneShot(hurt);
        yield return new WaitForSeconds(2f);
        BattleText.text = "Game Over.";
        TriggerLose();
    }

    IEnumerator RunWrongH()
    {
        yield return new WaitForSeconds(2f);
        BattleText.text = "You slipped!";
        yield return new WaitForSeconds(2f);
        HPText.text = "HP: 0";
        HPText.color = Color.red;
        BattleText.text = "The Goblin caught you.";
        audioSource.PlayOneShot(hurt);
        yield return new WaitForSeconds(2f);
        BattleText.text = "Game Over.";
        TriggerLose();
    }

    IEnumerator TimeUp()
    {
        BattleText.text = "You hesitated...";
        yield return new WaitForSeconds(2f);
        HPText.text = "HP: 0";
        HPText.color = Color.red;
        BattleText.text = "The Goblin strikes!";
        audioSource.PlayOneShot(hurt);
        yield return new WaitForSeconds(2f);
        BattleText.text = "Game Over.";
        TriggerLose();
    }

    void TriggerWin()
    {
        if (winOrLoseTriggered) return;
        winOrLoseTriggered = true;
        OnWin?.Invoke();
    }

    void TriggerLose()
    {
        if (winOrLoseTriggered) return;
        winOrLoseTriggered = true;
        OnLose?.Invoke();
    }

    public bool CheckWinCondition() => winOrLoseTriggered && Pressed && !OutofTime;
    public bool CheckLoseCondition() => winOrLoseTriggered && (!Pressed || OutofTime);
}