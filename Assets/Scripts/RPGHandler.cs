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

    // Start is called before the first frame update
    void Start()
    {
        int randomIndex = Random.Range(0, Situation.Length);
        chosenSituation = Situation[randomIndex];
        Debug.Log("Situation: " + chosenSituation);
        decisionTime = 3.0f;
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
            else
            {
                Debug.Log("WRONG");
                Pressed = true;
                BattleText.text = "The Goblin Attacks!";
                StartCoroutine(FightWrong());
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
                StartCoroutine(HealCorrect());
            }
            else
            {
                Debug.Log("WRONG");
                Pressed = true;
                BattleText.text = "The Goblin Attacks!";
                StartCoroutine(HealWrong());
            }
        }
        if(Input.GetKeyDown(KeyCode.DownArrow) && !Pressed && decisionTime >= 0.0f)
        {
            if(chosenSituation == "Run")
            {
                Debug.Log("CORRECT");
                Pressed = true;
                BattleText.text = "You ran away.";
            }
            else
            {
                Debug.Log("WRONG");
                Pressed = true;
                BattleText.text = "The Goblin Attacks!";
                StartCoroutine(RunWrong());
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



    IEnumerator FightCorrect()
	{
        yield return new WaitForSeconds(2f);
        BattleText.text = "The Goblin was defeated.";
        S_Goblin_256x256.SetActive(false);
        GoblinText.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        BattleText.text = "You Won!!!";
    }

    IEnumerator FightWrong()
	{
        yield return new WaitForSeconds(2f);
        HPText.text = "HP: 0";
        HPText.color = Color.red;
        BattleText.text = "You lost all your HP.";
        yield return new WaitForSeconds(2f);
        BattleText.text = "Game Over.";
    }

    IEnumerator HealCorrect()
	{
        yield return new WaitForSeconds(2f);
        BattleText.text = "The Goblin died due to poison.";
        S_Goblin_256x256.SetActive(false);
        GoblinText.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        BattleText.text = "You Won!!!";
    }

    IEnumerator HealWrong()
	{
        yield return new WaitForSeconds(2f);
        HPText.text = "HP: 0";
        HPText.color = Color.red;
        BattleText.text = "You lost all your HP.";
        yield return new WaitForSeconds(2f);
        BattleText.text = "Game Over.";
    }

    IEnumerator RunWrong()
	{
        yield return new WaitForSeconds(2f);
        HPText.text = "HP: 0";
        HPText.color = Color.red;
        BattleText.text = "You lost all your HP.";
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
        yield return new WaitForSeconds(2f);
        BattleText.text = "Game Over.";
    }
}
