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
        drawTime = Random.Range(3.0f, 6.0f);
        Debug.Log("Draw time: " + drawTime);
        wizardAnimator = WizardPrefab.GetComponent<Animator>();
        frogAnimator = FrogPrefab.GetComponent<Animator>();
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
                Wizard_Idle.SetActive(false);
                WizardPrefab.SetActive(true);
                audioSource.PlayOneShot(spell);
                pressed = true;
                Debug.Log("Too Early");
                audioSource.PlayOneShot(dead);
                }
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
                Debug.Log("WON");
            }
            else if (magicTime <= 0.0f)
            {
                QMText.text = "Gotcha!";
                pressed = true;
                audioSource.PlayOneShot(dead);
                Debug.Log("Game Over");
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
        Debug.Log("DRAW!!!");
    }
}
