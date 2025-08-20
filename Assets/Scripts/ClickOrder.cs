using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ClickOrder : MonoBehaviour
{
    public Clickable[] sprites;   // Assign clickable sprites
    public TMP_Text orderText;    // Assign a TextMeshPro UI element
    private List<int> order = new List<int>();
    private int currentIndex = 0;

    void Start()
    {
        GenerateRandomOrder();
    }

    void GenerateRandomOrder()
    {
        order.Clear();
        List<int> numbers = new List<int>();

        for (int i = 0; i < sprites.Length; i++)
        {
            numbers.Add(i);
        }

        // Shuffle list
        while (numbers.Count > 0)
        {
            int rand = Random.Range(0, numbers.Count);
            order.Add(numbers[rand]);
            numbers.RemoveAt(rand);
        }

        currentIndex = 0;

        // Build string using displayNames
        string display = "Order: ";
        for (int i = 0; i < order.Count; i++)
        {
            display += sprites[order[i]].displayName;
            if (i < order.Count - 1) display += " → ";
        }
        orderText.text = display;

        Debug.Log("New order: " + string.Join(", ", order));
    }

    public void CheckClick(int clickedIndex)
    {
        if (clickedIndex == order[currentIndex])
        {
            Debug.Log("Correct! Step " + currentIndex);
            currentIndex++;

            if (currentIndex >= order.Count)
            {
                Debug.Log("You Win! New Round!");
                GenerateRandomOrder(); // new round
            }
        }
        else
        {
            Debug.Log("Wrong! Restarting round...");
            GenerateRandomOrder();
        }
    }
}