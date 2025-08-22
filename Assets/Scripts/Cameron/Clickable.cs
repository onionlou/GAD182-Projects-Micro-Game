using UnityEngine;

public class Clickable : MonoBehaviour
{
    public int spriteIndex;       // Set in Inspector
    public string displayName;    // Friendly name for UI
    private ClickOrder gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<ClickOrder>();
    }

    void OnMouseDown()
    {
        if (gameManager != null)
        {
            gameManager.CheckClick(spriteIndex);
        }
    }
}
