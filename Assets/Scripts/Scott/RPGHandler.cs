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
    private bool Intro;
    private bool winOrLoseTriggered;

    public float decisionTime;

    public AudioSource audioSource;
    public AudioClip music;
    public AudioClip hit;
    public AudioClip heal;
    public AudioClip hurt;
    public AudioClip run;

    private int playerHP;
    private const int maxHP = 20;

    void Start()
    {
        int randomIndex = UnityEngine.Random.Range(0, Situation.Length);
        chosenSituation = Situation[randomIndex];
        Debug.Log("[RPG] Situation: " + chosenSituation);

        decisionTime = 11.0f;
        audioSource.PlayOneShot(music);
        Pressed = false;
        OutofTime = false;
        winOrLoseTriggered = false;

        if (chosenSituation == "Fight") StartCoroutine(IntroFight());
        else if (chosenSituation == "Heal") StartCoroutine(IntroHeal());
        else StartCoroutine(IntroRun());


        BattleText.text = "A goblin blocks your path.";

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

        if (decisionTime <= 0f && !Intro && !Pressed && !OutofTime)
        {
            Debug.Log("[RPG] TIME UP");
            OutofTime = true;
            StartCoroutine(TimeUp());
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && !Intro && !Pressed && decisionTime > 0f)
        {
            Pressed = true;
            BattleText.text = "You Attack the Goblin.";

            if (chosenSituation == "Fight") StartCoroutine(FightCorrect());
            else if (chosenSituation == "Heal") StartCoroutine(FightWrongH());
            else StartCoroutine(FightWrongR());
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && !Intro && !Pressed && decisionTime > 0f)
        {
            Pressed = true;
            BattleText.text = "You healed all your HP.";
            playerHP = maxHP;
            UpdateHPDisplay();
            audioSource.PlayOneShot(heal);

            if (chosenSituation == "Heal") StartCoroutine(HealCorrect());
            else if (chosenSituation == "Fight") StartCoroutine(HealWrongF());
            else StartCoroutine(HealWrongR());
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) && !Intro && !Pressed && decisionTime > 0f)
        {
            Pressed = true;
            BattleText.text = "You tried to run away.";

            if (chosenSituation == "Run") StartCoroutine(RunCorrect());
            else if (chosenSituation == "Fight") StartCoroutine(RunWrongF());
            else StartCoroutine(RunWrongH());
        }
    }

    void Fight()
    {
        playerHP = maxHP;
        UpdateHPDisplay();
        GoblinText.color = Color.red;
    }

    void Heal()
    {
        playerHP = 5;
        UpdateHPDisplay();
        GoblinText.color = Color.yellow;
    }

    void Run()
    {
        playerHP = 1;
        UpdateHPDisplay();
    }

    void UpdateHPDisplay()
    {
        HPText.text = $"HP: {playerHP}";
        if (playerHP > 9)
            HPText.color = Color.white;
        else if (playerHP > 4)
            HPText.color = Color.yellow;
        else
            HPText.color = Color.red;
    }

    IEnumerator IntroFight()
    {
        yield return new WaitForSeconds(3f);
        BattleText.text = "The Goblin is low on HP.";
        yield return new WaitForSeconds(3f);
        BattleText.text = "What do you do?\nLeft Arrow Key - Fight      Right Arrow Key - Heal    Down Arrow Key - Run";
        Intro = false;
    }

    IEnumerator IntroHeal()
    {
        yield return new WaitForSeconds(3f);
        BattleText.text = "Both you and the Goblin are poisoned.";
        yield return new WaitForSeconds(3f);
        BattleText.text = "What do you do?\nLeft Arrow Key - Fight      Right Arrow Key - Heal    Down Arrow Key - Run";
        Intro = false;
    }

    IEnumerator IntroRun()
    {
        yield return new WaitForSeconds(3f);
        BattleText.text = "Your low on HP and the Goblin is preparing it's super attack.";
        yield return new WaitForSeconds(3f);
        BattleText.text = "What do you do?\nLeft Arrow Key - Fight      Right Arrow Key - Heal    Down Arrow Key - Run";
        Intro = false;
    }

    IEnumerator FightCorrect() // Lucas: added null checks here for 
    {
        yield return new WaitForSeconds(2f);
        BattleText.text = "The Goblin was defeated.";
        audioSource.PlayOneShot(hit);

        if (S_Goblin_256x256 != null)
            S_Goblin_256x256.SetActive(false);
        else
            Debug.LogWarning("[RPG] Goblin sprite is missing!");

        if (GoblinText != null)
            GoblinText.gameObject.SetActive(false);
        else
            Debug.LogWarning("[RPG] GoblinText is missing!");

        yield return new WaitForSeconds(2f);
        BattleText.text = "You Won!!!";
        TriggerWin();
    }


    IEnumerator FightWrongH()
    {
        yield return new WaitForSeconds(2f);
        BattleText.text = "You Missed!";
        yield return new WaitForSeconds(2f);
        playerHP = 0;
        UpdateHPDisplay();
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
        playerHP = 0;
        UpdateHPDisplay();
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

        if (S_Goblin_256x256 != null)
            S_Goblin_256x256.SetActive(false);
        else
            Debug.LogWarning("[RPG] Goblin sprite is missing!");

        if (GoblinText != null)
            GoblinText.gameObject.SetActive(false);
        else
            Debug.LogWarning("[RPG] GoblinText is missing!");

        yield return new WaitForSeconds(2f);
        BattleText.text = "You Won!!!";
        TriggerWin();
    }

    IEnumerator HealWrongF()
    {
        yield return new WaitForSeconds(2f);
        BattleText.text = "The Goblin does a Super Attack!";
        yield return new WaitForSeconds(2f);
        playerHP = 0;
        UpdateHPDisplay();
        BattleText.text = "You lost all your HP.";
        audioSource.PlayOneShot(hurt);
        yield return new WaitForSeconds(2f);
        BattleText.text = "Game Over.";
        TriggerLose();
    }

    IEnumerator HealWrongR()
    {
        yield return new WaitForSeconds(2f);
        BattleText.text = "The Goblin does a Super Attack!";
        yield return new WaitForSeconds(2f);
        playerHP = 0;
        UpdateHPDisplay();
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

        if (S_Goblin_256x256 != null)
            S_Goblin_256x256.SetActive(false);
        else
            Debug.LogWarning("[RPG] Goblin sprite is missing!");

        if (GoblinText != null)
            GoblinText.gameObject.SetActive(false);
        else
            Debug.LogWarning("[RPG] GoblinText is missing!");

        yield return new WaitForSeconds(2f);
        BattleText.text = "You Won!!!";
        TriggerWin();
    }


    IEnumerator RunWrongF()
    {
        yield return new WaitForSeconds(2f);
        BattleText.text = "You tripped!";
        yield return new WaitForSeconds(2f);
        playerHP = 0;
        UpdateHPDisplay();
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
        playerHP = 0;
        UpdateHPDisplay();
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
        playerHP = 0;
        UpdateHPDisplay();
        BattleText.text = "The Goblin strikes!";
        audioSource.PlayOneShot(hurt);
        yield return new WaitForSeconds(2f);
        BattleText.text = "Game Over.";
        TriggerLose();
    }

    void TriggerWin() // Lucas: added debug logs to check if win is triggered correctly
    {
        if (winOrLoseTriggered) return;
        winOrLoseTriggered = true;

        Debug.Log("[RPG] TriggerWin() called");

        if (OnWin != null)
        {
            Debug.Log("[RPG] OnWin event is firing");
            OnWin.Invoke();
        }
        else
        {
            Debug.LogWarning("[RPG] OnWin has no subscribers");
        }
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
