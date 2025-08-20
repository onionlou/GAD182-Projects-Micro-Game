using UnityEngine;

public class Clickable : MonoBehaviour
{
    public int spriteIndex;         // Unique index assigned in inspector
    public string displayName;      // Name shown in the order UI

    private ClickOrder gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<ClickOrder>();
        if (gameManager == null)
        {
            Debug.LogError("Clickable: No ClickOrder found in scene.");
        }
    }

    void OnMouseDown()
    {
        if (gameManager != null)
        {
            gameManager.CheckClick(spriteIndex);
        }
    }
}