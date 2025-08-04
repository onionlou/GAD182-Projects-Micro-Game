using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RPGHandler : MonoBehaviour
{
    public TextMeshProUGUI BattleText;
    public TextMeshProUGUI HPText;
    public TextMeshProUGUI GoblinText;
    public static string[] Situation = {"Fight", "Heal", "Run"};
    private string chosenSituation;
    private bool Pressed;

    // Start is called before the first frame update
    void Start()
    {
        int randomIndex = Random.Range(0, Situation.Length);
        chosenSituation = Situation[randomIndex];
        Debug.Log("Situation: " + chosenSituation);
        Pressed = false;
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
        if(Input.GetKeyDown(KeyCode.LeftArrow) && !Pressed)
        {
            if(chosenSituation == "Fight")
            {
                Debug.Log("CORRECT");
                Pressed = true;
            }
            else
            {
                Debug.Log("WRONG");
                Pressed = true;
            }
        }
        if(Input.GetKeyDown(KeyCode.RightArrow) && !Pressed)
        {
            if(chosenSituation == "Heal")
            {
                Debug.Log("CORRECT");
                Pressed = true;
            }
            else
            {
                Debug.Log("WRONG");
                Pressed = true;
            }
        }
        if(Input.GetKeyDown(KeyCode.DownArrow) && !Pressed)
        {
            if(chosenSituation == "Run")
            {
                Debug.Log("CORRECT");
                Pressed = true;
            }
            else
            {
                Debug.Log("WRONG");
                Pressed = true;
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
}
