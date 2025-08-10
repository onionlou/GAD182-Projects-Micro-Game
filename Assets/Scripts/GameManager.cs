using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Canvas mainMenuCanvas;

    void Awake()
    {
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

        // Load Main Menu scene additively
        SceneSwapper.instance.LoadUnloadScene("Main Menu");
        Debug.Log("GameManager: Requesting Main Menu load");

        // Load UI overlays if any
        SceneSwapper.instance.OnSceneLoadComplete += OnUIScenesLoaded;
        SceneSwapper.instance.LoadStartingUI();
    }

    private void OnUIScenesLoaded()
    {
        Debug.Log("UI scenes loaded.");
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Debug.Log("Active scene: " + SceneManager.GetSceneAt(i).name);
        }
    }

    public void StartGame()
    {
        AudioManager.instance.PlayGameMusic();
        SceneSwapper.instance.StartGame();
    }

    public void ExitGame()
    {
        SceneSwapper.instance.ExitGame();
    }

    public void NextScene()
    {
        SceneSwapper.instance.LoadNextScene();
    }

    public void BackToMainMenu()
    {
        AudioManager.instance.PlayMainMenuMusic();
        SceneSwapper.instance.LoadUnloadScene("Main Menu");
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
