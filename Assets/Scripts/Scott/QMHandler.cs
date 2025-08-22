using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class QMHandler : MonoBehaviour, IWinCondition
{
    public event Action OnWin;
    public event Action OnLose;

    public GameObject QMHUD;
    public TextMeshProUGUI QMText;

    public float drawTime;
    public float magicTime;

    private bool drawn = false;
    private bool pressed = false;
    private bool winOrLoseTriggered = false;

    public GameObject WizardPrefab;
    public GameObject Wizard_Idle;
    public GameObject FrogPrefab;

    private Animator wizardAnimator;
    private Animator frogAnimator;

    public AudioSource audioSource;
    public AudioClip spell;
    public AudioClip ribbit;
    public AudioClip dead;

    void Start()
    {
        drawTime = UnityEngine.Random.Range(3.0f, 6.0f);
        Debug.Log("[QuickMagic] Draw time: " + drawTime);

        wizardAnimator = WizardPrefab.GetComponent<Animator>();
        frogAnimator = FrogPrefab.GetComponent<Animator>();
    }

    void Update()
    {
        if (winOrLoseTriggered) return;

        if (!drawn)
        {
            drawTime -= Time.deltaTime;

            if (drawTime <= 0.0f && !pressed)
            {
                draw();
            }
            else if (drawTime >= 0.0f && Input.GetKeyDown(KeyCode.Space))
            {
                QMText.text = "Gotcha!";
                Wizard_Idle.SetActive(false);
                WizardPrefab.SetActive(true);
                audioSource.PlayOneShot(spell);
                pressed = true;
                Debug.Log("[QuickMagic] Too Early");
                audioSource.PlayOneShot(dead);
                TriggerLose();
            }
        }
        else if (!pressed)
        {
            magicTime -= Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                QMText.text = "Ribbit";
                FrogPrefab.SetActive(true);
                WizardPrefab.SetActive(false);
                frogAnimator.Play("Anim_Frogl", 0, 0f);
                audioSource.PlayOneShot(ribbit);
                pressed = true;
                Debug.Log("[QuickMagic] WON");
                TriggerWin();
            }
            else if (magicTime <= 0.0f)
            {
                QMText.text = "Gotcha!";
                pressed = true;
                audioSource.PlayOneShot(dead);
                Debug.Log("[QuickMagic] Game Over");
                TriggerLose();
            }
        }
    }

    void draw()
    {
        magicTime = 2.0f;
        drawn = true;
        audioSource.PlayOneShot(spell);
        QMText.text = "Magic!";
        Wizard_Idle.SetActive(false);
        WizardPrefab.SetActive(true);
        wizardAnimator.Play("Anim_WizardSpell", 0, 0f);
        Debug.Log("[QuickMagic] DRAW!!!");
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

    public bool CheckWinCondition() => drawn && pressed && FrogPrefab.activeSelf;
    public bool CheckLoseCondition() => winOrLoseTriggered && !CheckWinCondition();
}