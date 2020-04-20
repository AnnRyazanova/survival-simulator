using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class SceneLoaderController : MonoBehaviour
{
    public static SceneLoaderController Instance => _instance;
    private static SceneLoaderController _instance;
    
    private readonly string _initScene = "Init";
    private readonly string _menuScene = "Menu/Menu";
    private readonly string _testScene = "MovementTest/MovementTest";

    public void LoadStartScene()
    {
        LoadScene(_testScene);
    }
    
    public void LoadMenuScene()
    {
        LoadScene(_menuScene);
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }
    
    private void Awake()
    {
        _instance = this;
        GameObject.DontDestroyOnLoad(gameObject);
        Init();
    }

    private void OnDestroy()
    {
        _instance = null;
    }

    private void Init() { }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        var name = string.Format($"Scenes/{sceneName}");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);

        while (!asyncLoad.isDone) {
            yield return null;
        }
    }
}
