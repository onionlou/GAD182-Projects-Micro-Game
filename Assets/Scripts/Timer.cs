using UnityEngine;
using TMPro; 

public class Timer : MonoBehaviour
{
    public float timeRemaining = 10f; 
    private bool countDown = true;     
    public TextMeshProUGUI timerText; 

    private void Update()
    {
        if (countDown)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                timeRemaining = 0;
                Debug.Log("Timer Finished!");
            }
        }
        else
        {
            timeRemaining += Time.deltaTime;
        }

        // Update TextMeshPro
        if (timerText != null)
        {
            timerText.text = timeRemaining.ToString("F2");
        }
    }
}