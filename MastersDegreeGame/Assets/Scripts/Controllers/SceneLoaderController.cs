using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class SceneLoaderController : MonoBehaviour
{
    public static SceneLoaderController Instance => _instance;
    private static SceneLoaderController _instance;
    
    private readonly string _initScene = "Init";
    private readonly string _menuScene = "Menu/Menu";
    private readonly string _testScene = "MovementTest/MovementTest";

    public LoadingWindow window;
    
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
        
        window.loadingScreen.SetActive(true);
        
        while (!asyncLoad.isDone) {
            float progress = Mathf.Clamp01(asyncLoad.progress / .9f);
            Debug.Log(progress);
            window.slider.value = progress;
            window.progressText.text = progress * 100f + "%";
            yield return null;
        }
    }
}
