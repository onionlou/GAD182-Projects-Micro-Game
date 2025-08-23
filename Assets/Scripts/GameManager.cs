using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public Canvas mainMenuCanvas;

    [Header("Game Progress")]
    public int totalMicrogames = 8;
    private int completedMicrogames = 0;

    [Header("UI Menus")]
    public GameObject winMenuUI;
    public GameObject loseMenuUI;
    public GameObject finalWinMenuUI;

    private bool gameEnded = false;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        var systems = FindObjectsOfType<EventSystem>();
        if (systems.Length > 1)
        {
            foreach (var es in systems)
            {
                if (es.gameObject != gameObject)
                    Destroy(es.gameObject);
            }
        }
    }

    void Start()
    {
        Debug.Log("GameManager: Start() called");
        AudioManager.instance.PlayMainMenuMusic();
        SceneSwapper.instance.LoadUnloadScene("Main Menu");
        SceneSwapper.instance.OnSceneLoadComplete += OnUIScenesLoaded;
        SceneSwapper.instance.LoadStartingUI();
        ResetPanels();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameEnded)
        {
            var uiManager = FindObjectOfType<UIManager>();
            if (uiManager != null)
            {
                if (Time.timeScale == 0f)
                    uiManager.HidePauseMenu();
                else
                    uiManager.ShowPauseMenu();
            }
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Debug: Manual trigger of HandleFinalWin via 'Z' key.");
            HandleFinalWin();
        }

        if (Input.GetKeyDown(KeyCode.X) && !gameEnded)
        {
            Debug.Log("Debug: Manual trigger of HandleWin via 'X' key.");
            HandleWin();
        }
    }

    private void OnUIScenesLoaded()
    {
        Debug.Log("GameManager: UI scenes loaded");
        ResetPanels();
        SubscribeToWinConditions();
    }

    private void SubscribeToWinConditions()
    {
        var winConditions = FindObjectsOfType<MonoBehaviour>().OfType<IWinCondition>().ToList();

        if (winConditions.Count == 0)
        {
            Debug.LogWarning("GameManager: No win conditions found in scene.");
            return;
        }

        foreach (var condition in winConditions)
        {
            condition.OnWin -= HandleWin;
            condition.OnLose -= HandleLose;
            condition.OnWin += HandleWin;
            condition.OnLose += HandleLose;
        }

        Debug.Log($"GameManager: Subscribed to {winConditions.Count} win conditions.");
    }

    private void HandleWin()
    {
        Debug.Log("GameManager: Entered HandleWin()");
        Debug.Log($"GameManager: gameEnded = {gameEnded}");

        if (gameEnded) return;
        gameEnded = true;

        completedMicrogames++;
        Debug.Log($"GameManager: Completed {completedMicrogames}/{totalMicrogames}");

        Time.timeScale = 0f;

        if (completedMicrogames >= totalMicrogames)
        {
            Debug.Log("GameManager: All microgames completed. Triggering final win...");
            StartCoroutine(EnsureFinalWinUIThenTrigger());
            return;
        }

        if (winMenuUI)
        {
            winMenuUI.SetActive(true);
            Debug.Log("GameManager: Win menu UI activated.");
        }

        AudioManager.instance.PlayWinSound(duckMusic: true, resumeAfter: true, duckDuration: 0.75f);
    }

    private void HandleLose()
    {
        Debug.Log("GameManager: Entered HandleLose()");
        Debug.Log($"GameManager: gameEnded = {gameEnded}");

        if (gameEnded) return;
        gameEnded = true;

        var uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            uiManager.ShowLoseUI();
            Debug.Log("GameManager: UIManager triggered lose UI.");
        }
        else
        {
            Debug.LogWarning("GameManager: UIManager not found. Using fallback loseMenuUI.");
            if (loseMenuUI) loseMenuUI.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    private IEnumerator EnsureFinalWinUIThenTrigger()
    {
        Debug.Log("GameManager: Checking finalWinMenuUI before triggering final win...");
        Debug.Log($"GameManager: finalWinMenuUI assigned = {finalWinMenuUI != null}");

        if (finalWinMenuUI == null || !finalWinMenuUI.scene.isLoaded)
        {
            Debug.Log("GameManager: Waiting for finalWinMenuUI to load...");
            yield return new WaitForSecondsRealtime(0.2f);
        }

        HandleFinalWin();
    }

    public void HandleFinalWin()
    {
        Debug.Log("GameManager: Entered HandleFinalWin()");
        Debug.Log($"GameManager: gameEnded = {gameEnded}");

        if (gameEnded) return;
        gameEnded = true;

        AudioManager.instance.StopMusic();

        if (finalWinMenuUI != null)
        {
            finalWinMenuUI.SetActive(true);
            Debug.Log("GameManager: Final win menu UI activated.");
        }
        else
        {
            Debug.LogWarning("GameManager: Final win menu UI is null!");
        }

        AudioManager.instance.PlayFinalWinSound();
        Time.timeScale = 0f;
    }

    // -----------------------------
    // UI Buttons
    // -----------------------------

    public void StartGame()
    {
        Debug.Log("GameManager: StartGame() called");
        completedMicrogames = 0;
        ResetGameState();
        HideMainMenuCanvas();
        AudioManager.instance.PlayGameMusic();
        SceneSwapper.instance.StartGame();
    }

    public void ExitGame()
    {
        Debug.Log("GameManager: ExitGame() called");
        SceneSwapper.instance.ExitGame();
    }

    public void NextScene()
    {
        Debug.Log("GameManager: NextScene() called");
        ResetGameState();
        AudioManager.instance.PlayGameMusic();
        SceneSwapper.instance.LoadNextScene();
    }

    public void RestartScene()
    {
        Debug.Log("GameManager: RestartScene() called");
        ResetGameState();
        AudioManager.instance.PlayGameMusic();

        string currentScene = SceneManager.GetActiveScene().name;
        Debug.Log($"GameManager: Restarting scene {currentScene}");

        StartCoroutine(ReloadMicrogameScene(currentScene));
    }

    private IEnumerator ReloadMicrogameScene(string sceneName)
    {
        Debug.Log($"GameManager: Unloading scene {sceneName}");
        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(sceneName);
        while (!unloadOp.isDone)
            yield return null;

        Debug.Log($"GameManager: Reloading scene {sceneName}");
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!loadOp.isDone)
            yield return null;

        Debug.Log($"GameManager: Scene {sceneName} reloaded");

        yield return new WaitForSecondsRealtime(0.1f);
        SubscribeToWinConditions();
    }

    public void BackToMainMenu()
    {
        Debug.Log("GameManager: BackToMainMenu() called");
        completedMicrogames = 0;
        ResetGameState();
        if (mainMenuCanvas != null) mainMenuCanvas.gameObject.SetActive(true);
        AudioManager.instance.PlayMainMenuMusic();
        SceneSwapper.instance.LoadUnloadScene("Main Menu");
    }

    // -----------------------------
    // Helpers
    // -----------------------------

    private void ResetGameState()
    {
        Debug.Log("GameManager: ResetGameState() called");
        gameEnded = false;
        Time.timeScale = 1f;
        ResetPanels();
    }

    private void ResetPanels()
    {
        Debug.Log("GameManager: ResetPanels() called");
        if (winMenuUI) winMenuUI.SetActive(false);
        if (loseMenuUI) loseMenuUI.SetActive(false);
        if (finalWinMenuUI) finalWinMenuUI.SetActive(false);
    }

    private void HideMainMenuCanvas()
    {
        if (mainMenuCanvas != null && mainMenuCanvas.gameObject.activeSelf)
        {
            mainMenuCanvas.gameObject.SetActive(false);
            Debug.Log("GameManager: Main menu canvas hidden.");
        }
    }
}