using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ClickOrder : MonoBehaviour, IWinCondition
{
    public event Action OnWin;
    public event Action OnLose;

    public Clickable[] sprites;       // Assign clickable sprites in Inspector
    public TMP_Text orderText;        // Assign a TextMeshPro UI element

    [Header("Game Settings")]
    public float timeLimit = 30f;     // Total time to complete 3 rounds

    private List<int> order = new List<int>();
    private int currentIndex = 0;
    private int completedRounds = 0;
    private bool winOrLoseTriggered = false;

    void Start()
    {
        GenerateRandomOrder();
    }

    void Update()
    {
        if (winOrLoseTriggered) return;

        timeLimit -= Time.deltaTime;

        if (timeLimit <= 0f)
        {
            Debug.Log("[ClickOrder] Time ran out — triggering lose.");
            TriggerLose();
        }
    }

    void GenerateRandomOrder()
    {
        order.Clear();

        List<int> numbers = new List<int>();
        for (int i = 0; i < sprites.Length; i++)
        {
            numbers.Add(i);
        }

        while (numbers.Count > 0)
        {
            int rand = UnityEngine.Random.Range(0, numbers.Count);
            order.Add(numbers[rand]);
            numbers.RemoveAt(rand);
        }

        currentIndex = 0;

        string display = "Order: ";
        for (int i = 0; i < order.Count; i++)
        {
            display += sprites[order[i]].displayName;
            if (i < order.Count - 1) display += " → ";
        }

        if (orderText != null)
            orderText.text = display;

        Debug.Log("[ClickOrder] New order: " + string.Join(", ", order));
    }

    public void CheckClick(int clickedIndex)
    {
        if (winOrLoseTriggered) return;

        if (clickedIndex == order[currentIndex])
        {
            Debug.Log("[ClickOrder] Correct! Step " + currentIndex);
            currentIndex++;

            if (currentIndex >= order.Count)
            {
                completedRounds++;
                Debug.Log($"[ClickOrder] Round complete! Total correct rounds: {completedRounds}");

                if (completedRounds >= 3)
                {
                    TriggerWin();
                }
                else
                {
                    GenerateRandomOrder();
                }
            }
        }
        else
        {
            Debug.Log("[ClickOrder] Wrong! Restarting round...");
            GenerateRandomOrder();
        }
    }

    void TriggerWin()
    {
        if (winOrLoseTriggered) return;
        winOrLoseTriggered = true;
        Debug.Log("[ClickOrder] ✔ Win condition met.");
        OnWin?.Invoke();
    }

    void TriggerLose()
    {
        if (winOrLoseTriggered) return;
        winOrLoseTriggered = true;
        Debug.Log("[ClickOrder] ✘ Lose condition met.");
        OnLose?.Invoke();
    }

    public bool CheckWinCondition() => winOrLoseTriggered && completedRounds >= 3;
    public bool CheckLoseCondition() => winOrLoseTriggered && completedRounds < 3;
}