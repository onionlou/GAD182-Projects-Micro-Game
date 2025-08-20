using UnityEngine;

public class Clickable : MonoBehaviour
{
    public int spriteIndex;   
    public string displayName; 

    private ClickOrder gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<ClickOrder>();
    }

    void OnMouseDown()
    {
        gameManager.CheckClick(spriteIndex);
    }
}