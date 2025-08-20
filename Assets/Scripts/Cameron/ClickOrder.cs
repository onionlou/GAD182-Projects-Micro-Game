using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

public class ClickOrder : MonoBehaviour, IWinCondition
{
    public event Action OnWin;
    public event Action OnLose;

    public Clickable[] sprites;
    public TMP_Text orderText;
    public float timeLimit = 8f;

    private List<int> order = new List<int>();
    private int currentIndex = 0;
    private float timer;
    private bool gameActive = true;
    private bool hasWon = false;
    private bool hasLost = false;

    void Start()
    {
        timer = timeLimit;
        GenerateRandomOrder();
    }

    void Update()
    {
        if (!gameActive) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            hasLost = true;
            gameActive = false;
            OnLose?.Invoke();
            Debug.Log("ClickOrder: Time ran out — Lose triggered.");
        }
    }

    void GenerateRandomOrder()
    {
        order.Clear();
        List<int> numbers = new List<int>();
        for (int i = 0; i < sprites.Length; i++) numbers.Add(i);

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
        orderText.text = display;
    }

    public void CheckClick(int clickedIndex)
    {
        if (!gameActive) return;

        if (clickedIndex == order[currentIndex])
        {
            currentIndex++;
            if (currentIndex >= order.Count)
            {
                hasWon = true;
                gameActive = false;
                OnWin?.Invoke();
                Debug.Log("ClickOrder: Win triggered!");
            }
        }
        else
        {
            hasLost = true;
            gameActive = false;
            OnLose?.Invoke();
            Debug.Log("ClickOrder: Wrong order — Lose triggered.");
        }
    }

    public bool CheckWinCondition() => hasWon;
    public bool CheckLoseCondition() => hasLost;
}