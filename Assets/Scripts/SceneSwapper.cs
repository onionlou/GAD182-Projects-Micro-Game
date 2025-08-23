using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwapper : MonoBehaviour
{
    private enum OperationType { load, unload }

    private class SceneLoadWrapper
    {
        public string scene;
        public OperationType operation;

        public SceneLoadWrapper(string scene, OperationType operation)
        {
            this.scene = scene;
            this.operation = operation;
        }
    }

    public static SceneSwapper instance;

    [Tooltip("UI overlay scenes to load additively.")]
    public string[] uiScenes;

    [Tooltip("Game scenes to cycle through.")]
    public string[] gameScenes;

    private Queue sceneQueue = new Queue();
    private bool loadingScenes = false;
    private AsyncOperation currentOperation;
    public string CurrentScene { get; private set; }
    private int currentGameSceneIndex = -1;

    public event System.Action OnSceneLoadComplete;

    private HashSet<string> loadedScenes = new HashSet<string>();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadStartingUI()
    {
        if (uiScenes == null || uiScenes.Length == 0) return;

        foreach (string sceneName in uiScenes)
        {
            if (!loadedScenes.Contains(sceneName))
            {
                sceneQueue.Enqueue(new SceneLoadWrapper(sceneName, OperationType.load));
            }
            else
            {
                Debug.Log($"Scene '{sceneName}' already loaded. Skipping.");
            }
        }

        StartLoadQueue();
    }

    private HashSet<string> queuedScenes = new HashSet<string>();

    public void LoadUnloadScene(string sceneToLoad)
    {
        if (string.IsNullOrEmpty(sceneToLoad)) return;

        if (!queuedScenes.Contains(sceneToLoad))
        {
            if (!string.IsNullOrEmpty(CurrentScene) && loadedScenes.Contains(CurrentScene) && sceneToLoad != CurrentScene)
            {
                sceneQueue.Enqueue(new SceneLoadWrapper(CurrentScene, OperationType.unload));
                queuedScenes.Add(CurrentScene);
            }

            if (!loadedScenes.Contains(sceneToLoad))
            {
                sceneQueue.Enqueue(new SceneLoadWrapper(sceneToLoad, OperationType.load));
                queuedScenes.Add(sceneToLoad);
            }

            CurrentScene = sceneToLoad;
            StartLoadQueue();
        }
    }

    public void LoadScene(int positionInList)
    {
        if (gameScenes == null || gameScenes.Length == 0) return;

        if (positionInList < gameScenes.Length)
        {
            LoadUnloadScene(gameScenes[positionInList]);
            currentGameSceneIndex = positionInList;
        }
        else
        {
            Debug.LogError($"No game scene at index {positionInList}");
        }
    }

    public void LoadNextScene()
    {
        if (gameScenes == null || gameScenes.Length == 0) return;

        currentGameSceneIndex = (currentGameSceneIndex + 1) % gameScenes.Length;
        LoadScene(currentGameSceneIndex);
    }

    public void StartGame()
    {
        if (gameScenes.Length > 0)
        {
            LoadScene(0);
        }
        else
        {
            Debug.LogWarning("No game scenes assigned in SceneSwapper.");
        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void StartLoadQueue()
    {
        if (!loadingScenes)
        {
            StartCoroutine(SceneLoadQueue());
        }
    }

    private IEnumerator SceneLoadQueue()
    {
        loadingScenes = true;
        while (sceneQueue.Count > 0)
        {
            if (currentOperation == null || currentOperation.isDone)
            {
                SceneLoadWrapper nextScene = (SceneLoadWrapper)sceneQueue.Peek();
                yield return StartCoroutine(WaitForScene(nextScene));
            }
            yield return null;
        }
        loadingScenes = false;
        OnSceneLoadComplete?.Invoke();
    }

    private IEnumerator WaitForScene(SceneLoadWrapper wrapper)
    {
        Debug.Log($"SceneSwapper: {wrapper.operation} scene '{wrapper.scene}'");

        if (wrapper.operation == OperationType.load)
        {
            currentOperation = SceneManager.LoadSceneAsync(wrapper.scene, LoadSceneMode.Additive);
            yield return new WaitUntil(() => SceneManager.GetSceneByName(wrapper.scene).isLoaded);

            Scene loadedScene = SceneManager.GetSceneByName(wrapper.scene);
            if (loadedScene.IsValid())
            {
                SceneManager.SetActiveScene(loadedScene);
                Debug.Log($"SceneSwapper: Activated scene '{wrapper.scene}'");
            }

            loadedScenes.Add(wrapper.scene);
        }
        else
        {
            currentOperation = SceneManager.UnloadSceneAsync(wrapper.scene);
            yield return new WaitUntil(() => !SceneManager.GetSceneByName(wrapper.scene).isLoaded);

            loadedScenes.Remove(wrapper.scene);
        }

        sceneQueue.Dequeue();
    }

    public List<string> GetActiveScenes()
    {
        List<string> activeScenes = new List<string>();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            activeScenes.Add(SceneManager.GetSceneAt(i).name);
        }
        return activeScenes;
    }
    // Called by the "Play Again" button
    public void RestartCurrentScene()
    {
        if (!string.IsNullOrEmpty(CurrentScene))
        {
            Debug.Log($"SceneSwapper: Restarting scene '{CurrentScene}'");
            LoadUnloadScene(CurrentScene); // Re-load the same scene
        }
        else
        {
            Debug.LogWarning("SceneSwapper: No current scene to restart.");
        }
    }

    // Called by the "Exit" button
    public void ExitToDesktop()
    {
        Debug.Log("SceneSwapper: Exiting game via Victory Panel.");
        ExitGame();
    }

}