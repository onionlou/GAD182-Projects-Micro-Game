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

    [Header("Scene Transition")]
    [SerializeField] private float winDelay = 2f;

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

        StartCoroutine(DelayedSceneSwap());
    }

    private void HandleLose()
    {
        if (gameEnded) return;
        gameEnded = true;

        Debug.Log("GameManager: HandleLose() triggered!");
        Time.timeScale = 0f;

        if (loseMenuUI) loseMenuUI.SetActive(true);
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

    private IEnumerator DelayedSceneSwap()
    {
        yield return new WaitForSecondsRealtime(winDelay);
        Time.timeScale = 1f;
        gameEnded = false;
        SceneSwapper.instance.LoadNextScene();
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
        var currentScene = SceneManager.GetActiveScene().name;
        SceneSwapper.instance.LoadUnloadScene(currentScene);
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



/* PREVIOUS SCRIPT VERSION
//To ensure that there's no conflicts on loading, we tell the SceneSwapper to load our scenes from Start here, to prevent script load execution errors
void Start()
    {
        //Tell SceneSwapper to load the starting UI
        SceneSwapper.instance.LoadStartingUI();
        //RandomSelectScene();
        SelectScene("5 BulletHell Game");
    }

    public void RandomSelectScene()
    {
        int random = Random.Range(0, SceneSwapper.instance.gameScenes.Length);

        Debug.Log(random);
        //Also tell SceneSwapper to load the game scene at position 0
        SceneSwapper.instance.LoadScene(random);
    }QA

    public void SelectScene(string sceneName)
    {
        SceneSwapper.instance.LoadUnloadScene(sceneName);
    }

    /*public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneSwapper.instance.LoadUnloadScene(SceneSwapper.instance.CurrentScene);
            RandomSelectScene();
        }
    } */
