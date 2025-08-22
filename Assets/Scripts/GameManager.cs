using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public Canvas mainMenuCanvas;

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

    private void Start()
    {
        AudioManager.instance.PlayMainMenuMusic();
        SceneSwapper.instance.LoadUnloadScene("Main Menu");
        Debug.Log("GameManager: Requesting Main Menu load");

        SceneSwapper.instance.OnSceneLoadComplete += OnUIScenesLoaded;
        SceneSwapper.instance.LoadStartingUI();

        ResetPanels();
    }
    private void HideMainMenuCanvas()
    {
        if (mainMenuCanvas != null && mainMenuCanvas.gameObject.activeSelf)
        {
            mainMenuCanvas.gameObject.SetActive(false);
            Debug.Log("Main menu canvas hidden.");
        }
    }

    private void OnUIScenesLoaded()
    {
        ResetPanels();
        Debug.Log("UI scenes loaded.");
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Debug.Log("Active scene: " + SceneManager.GetSceneAt(i).name);
        }
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
            condition.OnWin += HandleWin;
            condition.OnLose += HandleLose;
        }
    }

    private void HandleWin()
    {
        if (gameEnded) return;
        gameEnded = true;

        Debug.Log("GameManager: HandleWin() triggered!");
        Time.timeScale = 0f;

        if (winMenuUI) winMenuUI.SetActive(true);
    }


    private void HandleLose()
    {
        if (gameEnded) return;
        gameEnded = true;

        Debug.Log("GameManager: HandleLose() triggered!");

        // Pause and show lose UI via UIManager
        var uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            uiManager.ShowLoseUI();
        }
        else
        {
            Debug.LogWarning("GameManager: UIManager not found. Showing fallback loseMenuUI.");
            if (loseMenuUI) loseMenuUI.SetActive(true);
            Time.timeScale = 0f;
        }
    }


    public void HandleFinalWin()
    {
        if (gameEnded) return;
        gameEnded = true;

        Debug.Log("GameManager: HandleFinalWin() triggered!");
        Time.timeScale = 0f;

        if (finalWinMenuUI) finalWinMenuUI.SetActive(true);
        AudioManager.instance.PlayMainMenuMusic();
    }

    // -----------------------------
    // UI Buttons
    // -----------------------------

    public void StartGame()
    {
        ResetGameState();
        HideMainMenuCanvas();
        AudioManager.instance.PlayGameMusic();
        SceneSwapper.instance.StartGame();
    }

    public void ExitGame()
    {
        SceneSwapper.instance.ExitGame();
    }

    public void NextScene()
    {
        ResetGameState();
        SceneSwapper.instance.LoadNextScene();
    }
    public void RestartScene()
    {
        ResetGameState();

        string currentScene = SceneManager.GetActiveScene().name;
        Debug.Log($"Restarting microgame scene: {currentScene}");

        StartCoroutine(ReloadMicrogameScene(currentScene));
    }

    private IEnumerator ReloadMicrogameScene(string sceneName)
    {
        // Unload current microgame scene
        AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(sceneName);
        while (!unloadOp.isDone)
            yield return null;

        // Reload it additively
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!loadOp.isDone)
            yield return null;

        Debug.Log($"Microgame scene {sceneName} reloaded.");
    }

    public void BackToMainMenu()
    {
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
        gameEnded = false;
        Time.timeScale = 1f;
        ResetPanels();
    }

    private void ResetPanels()
    {
        if (winMenuUI) winMenuUI.SetActive(false);
        if (loseMenuUI) loseMenuUI.SetActive(false);
        if (finalWinMenuUI) finalWinMenuUI.SetActive(false);
    }
}