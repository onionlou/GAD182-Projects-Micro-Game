using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
    }

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
}
