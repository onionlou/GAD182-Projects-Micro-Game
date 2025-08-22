using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject winMenuPanel;
    [SerializeField] private GameObject pausePanel;

    [Header("Optional Buttons")]
    public Button nextButton;   // For progressing to the next game
    public Button retryButton;  // For retrying the same game

    private void Awake()
    {
        // Ensure panels start hidden
        ResetPanels();
    }

    public void ShowWinPanel() => winPanel?.SetActive(true);
    public void ShowLosePanel() => losePanel?.SetActive(true);
    public void ShowFinalWinMenu() => winMenuPanel?.SetActive(true);

    public void ShowWinUI()
    {
        winPanel?.SetActive(true);
        losePanel?.SetActive(false);
        PauseGame();
        Debug.Log("Win UI displayed!");
    }

    public void ShowLoseUI()
    {
        losePanel?.SetActive(true);
        winPanel?.SetActive(false);
        PauseGame();
        Debug.Log("Lose UI displayed!");
    }

    public void HideUI()
    {
        winPanel?.SetActive(false);
        losePanel?.SetActive(false);
    }

    public void ResetPanels()
    {
        winPanel?.SetActive(false);
        losePanel?.SetActive(false);
        winMenuPanel?.SetActive(false);
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        Debug.Log("Game paused.");
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        Debug.Log("Game resumed.");
    }
    public void ShowPauseMenu()
    {
        pausePanel?.SetActive(true);
        PauseGame();
    }

    public void HidePauseMenu()
    {
        pausePanel?.SetActive(false);
        ResumeGame();
    }

    public void ExitToMainMenuFromPause()
    {
        ResumeGame();
        FindObjectOfType<GameManager>()?.BackToMainMenu();
    }
}