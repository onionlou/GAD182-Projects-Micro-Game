using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QMHandler : MonoBehaviour
{
    public GameObject QMHUD;
    public TextMeshProUGUI QMText;

    public float drawTime;
    public float magicTime;

    private bool drawn = false;
    private bool pressed = false;

    void Start()
    {
        drawTime = Random.Range(3.0f, 6.0f);
        Debug.Log("Draw time: " + drawTime);
    }

    void Update()
    {
        if (!drawn)
        {
            drawTime -= Time.deltaTime;

            if (drawTime <= 0.0f && !pressed)
            {
                draw();
            }
            else if (drawTime >= 0.0f)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                QMText.text = "Gotcha!";
                pressed = true;
                Debug.Log("Too Early");
                }
            }
        }
        else if (!pressed)
        {
            magicTime -= Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                QMText.text = "Ribbit";
                pressed = true;
                Debug.Log("WON");
            }
            else if (magicTime <= 0.0f)
            {
                QMText.text = "Gotcha!";
                pressed = true;
                Debug.Log("Game Over");
            }
        }
    }

    void draw()
    {
        magicTime = 2.0f;
        drawn = true;
        QMText.text = "Magic!";
        Debug.Log("DRAW!!!");
    }
}
