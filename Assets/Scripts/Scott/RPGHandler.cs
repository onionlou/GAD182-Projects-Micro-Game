using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RPGHandler : MonoBehaviour
{
    public TextMeshProUGUI BattleText;
    public TextMeshProUGUI HPText;
    public TextMeshProUGUI GoblinText;
    public GameObject S_Goblin_256x256;
    public static string[] Situation = {"Fight", "Heal", "Run"};
    private string chosenSituation;
    private bool Pressed;
    public float decisionTime;
    private bool OutofTime;

    public AudioSource audioSource;
    public AudioClip music;
    public AudioClip hit;
    public AudioClip heal;
    public AudioClip hurt;
    public AudioClip run;

    // Start is called before the first frame update
    void Start()
    {
        int randomIndex = Random.Range(0, Situation.Length);
        chosenSituation = Situation[randomIndex];
        Debug.Log("Situation: " + chosenSituation);
        decisionTime = 5.0f;
        audioSource.PlayOneShot(music);
        Pressed = false;
        OutofTime = false;
        BattleText.text = "A goblin blocks your path. What do you do?\nLeft Arrow Key - Fight                         Right Arrow Key - Heal\n                             Down Arrow Key - Run";

        switch (chosenSituation)
        {
            case "Fight":
                Fight();
                break;

            case "Heal":
                Heal();
                break;

            case "Run":
                Run();
                break;

            default:
                Debug.Log("Error");
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        decisionTime -= Time.deltaTime;

        if (decisionTime <= 0.0f && !Pressed && !OutofTime)
        {
            Debug.Log("TIME UP");
            OutofTime = true;
            StartCoroutine(TimeUp());
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow) && !Pressed && decisionTime >= 0.0f)
        {
            if(chosenSituation == "Fight")
            {
                Debug.Log("CORRECT");
                Pressed = true;
                BattleText.text = "You Attack the Goblin.";
                StartCoroutine(FightCorrect());
            }
            if(chosenSituation == "Heal")
            {
                Debug.Log("WRONG");
                Pressed = true;
                BattleText.text = "You Attack the Goblin.";
                StartCoroutine(FightWrongH());
            }
            if(chosenSituation == "Run")
            {
                Debug.Log("WRONG");
                Pressed = true;
                BattleText.text = "You Attack the Goblin.";
                StartCoroutine(FightWrongR());
            }
        }
        if(Input.GetKeyDown(KeyCode.RightArrow) && !Pressed && decisionTime >= 0.0f)
        {
            if(chosenSituation == "Heal")
            {
                Debug.Log("CORRECT");
                Pressed = true;
                BattleText.text = "You healed all your HP.";
                HPText.text = "HP: 20";
                HPText.color = Color.white;
                audioSource.PlayOneShot(heal);
                StartCoroutine(HealCorrect());
            }
            if(chosenSituation == "Fight")
            {
                Debug.Log("WRONG");
                Pressed = true;
                BattleText.text = "You healed all your HP.";
                HPText.text = "HP: 20";
                HPText.color = Color.white;
                audioSource.PlayOneShot(heal);
                StartCoroutine(HealWrongF());
            }
            if(chosenSituation == "Run")
            {
                Debug.Log("WRONG");
                Pressed = true;
                BattleText.text = "You healed all your HP.";
                HPText.text = "HP: 20";
                HPText.color = Color.white;
                audioSource.PlayOneShot(heal);
                StartCoroutine(HealWrongR());
            }
        }
        if(Input.GetKeyDown(KeyCode.DownArrow) && !Pressed && decisionTime >= 0.0f)
        {
            if(chosenSituation == "Run")
            {
                Debug.Log("CORRECT");
                Pressed = true;
                BattleText.text = "You tried to run away.";
                StartCoroutine(RunCorrect());
            }
            if(chosenSituation == "Fight")
            {
                Debug.Log("WRONG");
                Pressed = true;
                BattleText.text = "You tried to run away.";
                StartCoroutine(RunWrongF());
            }
            if(chosenSituation == "Heal")
            {
                Debug.Log("WRONG");
                Pressed = true;
                BattleText.text = "You tried to run away.";
                StartCoroutine(RunWrongH());
            }
        }
    }

    //this just displays info
    void Fight()
    {
        HPText.text = "HP: 20";
        GoblinText.color = Color.red;
    }

    void Heal()
    {
        HPText.text = "HP: 5";
        HPText.color = Color.yellow;
        GoblinText.color = Color.yellow;
    }

    void Run()
    {
        HPText.text = "HP: 1";
        HPText.color = Color.red;
    }


    //chose fight and was correct
    IEnumerator FightCorrect()
	{
        yield return new WaitForSeconds(2f);
        BattleText.text = "The Goblin was defeated.";
        audioSource.PlayOneShot(hit);
        S_Goblin_256x256.SetActive(false);
        GoblinText.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        BattleText.text = "You Won!!!";
    }

     //chose fight during heal
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
    }

    //chose fight during run
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
    }

    //chose heal and was correct
    IEnumerator HealCorrect()
	{
        yield return new WaitForSeconds(2f);
        BattleText.text = "The Goblin died due to poison.";
        audioSource.PlayOneShot(hit);
        S_Goblin_256x256.SetActive(false);
        GoblinText.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        BattleText.text = "You Won!!!";
    }

    //chose heal during fight
    IEnumerator HealWrongF()
	{
        yield return new WaitForSeconds(2f);
        BattleText.text = "The Goblin Uses It's Super Attack!!";
        yield return new WaitForSeconds(2f);
        HPText.text = "HP: 0";
        HPText.color = Color.red;
        BattleText.text = "You lost all your HP.";
        audioSource.PlayOneShot(hurt);
        yield return new WaitForSeconds(2f);
        BattleText.text = "Game Over.";
    }

    //chose heal during run
    IEnumerator HealWrongR()
	{
        yield return new WaitForSeconds(2f);
        BattleText.text = "The Goblin Uses It's Super Attack!!";
        yield return new WaitForSeconds(2f);
        HPText.text = "HP: 0";
        HPText.color = Color.red;
        BattleText.text = "You lost all your HP.";
        audioSource.PlayOneShot(hurt);
        yield return new WaitForSeconds(2f);
        BattleText.text = "Game Over.";
    }

    //chose run and was correct
    IEnumerator RunCorrect()
	{
        yield return new WaitForSeconds(2f);
        BattleText.text = "...";
        yield return new WaitForSeconds(2f);
        BattleText.text = "You Got Away Safely!";
        audioSource.PlayOneShot(run, 1.0f);
    }

    //chose run during fight
    IEnumerator RunWrongF()
	{
        yield return new WaitForSeconds(2f);
        BattleText.text = "...";
        yield return new WaitForSeconds(2f);
        BattleText.text = "You Failed.";
        yield return new WaitForSeconds(2f);
        BattleText.text = "The Goblin Attacks!";
        yield return new WaitForSeconds(2f);
        HPText.text = "HP: 0";
        HPText.color = Color.red;
        BattleText.text = "You lost all your HP.";
        audioSource.PlayOneShot(hurt);
        yield return new WaitForSeconds(2f);
        BattleText.text = "Game Over.";
    }

    //chose run during heal
    IEnumerator RunWrongH()
	{
        yield return new WaitForSeconds(2f);
        BattleText.text = "...";
        yield return new WaitForSeconds(2f);
        BattleText.text = "You Failed.";
        yield return new WaitForSeconds(2f);
        BattleText.text = "The Goblin Attacks!";
        yield return new WaitForSeconds(2f);
        HPText.text = "HP: 0";
        HPText.color = Color.red;
        BattleText.text = "You lost all your HP.";
        audioSource.PlayOneShot(hurt);
        yield return new WaitForSeconds(2f);
        BattleText.text = "Game Over.";
    }


    IEnumerator TimeUp()
	{
        yield return new WaitForSeconds(0.1f);
        BattleText.text = "You took too long.";
        yield return new WaitForSeconds(2f);
        BattleText.text = "The Goblin Attacks!";
        yield return new WaitForSeconds(2f);
        HPText.text = "HP: 0";
        HPText.color = Color.red;
        BattleText.text = "You lost all your HP.";
        audioSource.PlayOneShot(hurt);
        yield return new WaitForSeconds(2f);
        BattleText.text = "Game Over.";
    }
}
