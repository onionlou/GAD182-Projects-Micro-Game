using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject winMenuPanel;


    [Header("Optional Buttons")]
    public Button nextButton;   // For progressing to the next game
    public Button retryButton;  // For retrying the same game

    public void ShowWinPanel() => winPanel.SetActive(true);
    public void ShowLosePanel() => losePanel.SetActive(true);
    public void ShowFinalWinMenu() => winMenuPanel.SetActive(true);

    private void Awake()
    {
        // Ensure panels start hidden
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
    }

    public void ShowWinUI()
    {
        if (winPanel != null) winPanel.SetActive(true);
        if (losePanel != null) losePanel.SetActive(false);
        Debug.Log("Win UI displayed!");
    }

    public void ShowLoseUI()
    {
        if (losePanel != null) losePanel.SetActive(true);
        if (winPanel != null) winPanel.SetActive(false);
        Debug.Log("Lose UI displayed!");
    }

    public void HideUI()
    {
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
    }
    public void ResetPanels()
    {
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
        if (winMenuPanel != null) winMenuPanel.SetActive(false);
    }
}
