using System.Collections;
using System.Collections.Generic;
using SceneGeneration.PerlinNoise;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class SceneLoaderController : MonoBehaviour
{
    public static SceneLoaderController Instance => _instance;
    private static SceneLoaderController _instance;
    
    private readonly string _menuScene = "Menu/Menu";
    private readonly string _testScene = "MovementTest/MovementTest";
    private readonly string _testGenScene = "Generator/Perlin";
    
    public void LoadStartScene(int val)
    {
        var scene = "";
        
        switch (val) {
            case 0:
                scene = _testScene;
                break;
            case 1:
                scene = _testGenScene;
                break;
            default:
                scene = _testScene;
                break;
        }
        
        LoadScene(scene);
    }
    
    public void LoadMenuScene(bool showLoaderWindow, bool showMenuWindow)
    {
        LoadScene(_menuScene, showLoaderWindow, showMenuWindow);
    }

    public void LoadScene(string sceneName, bool showLoaderWindow = true, bool showMenuWindow = true)
    {
        StartCoroutine(LoadSceneAsync(sceneName, showLoaderWindow, showMenuWindow));
    }
    
    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
        Init();
    }

    private void OnDestroy()
    {
        _instance = null;
    }

    private void Init() { }

    private IEnumerator LoadSceneAsync(string sceneName, bool showLoaderWindow = true, bool showMenuWindow = true)
    {
        var name = string.Format($"Scenes/{sceneName}");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);

        if (showLoaderWindow) {
            LoadingWindowController.Instance.ShowWindow();
        }

#if CHEAT
        float delayedProgress = 0;
        var delay = (showLoaderWindow == true && delayedProgress > .99) || showLoaderWindow == false;
#endif

        while (asyncLoad.isDone == false 
#if CHEAT
         || delay == false
#endif
            ) {
            if (showLoaderWindow) {
                float progress = Mathf.Clamp01(asyncLoad.progress / .9f);
                
#if CHEAT
                delayedProgress += .01f;
                progress = delayedProgress;
                delay = (showLoaderWindow == true && delayedProgress > .99) || showLoaderWindow == false;
#endif
                
                LoadingWindowController.Instance.SetProgress(progress);
            }
            yield return null;
        }

        OnSceneLoaded(showMenuWindow);
    }

    private void OnSceneLoaded(bool showMenuWindow = true)
    {
        LoadingWindowController.Instance.HideWindow();

        if (showMenuWindow) {
            MainWindowController.Instance.ShowWindow();
            InstantiatePrefabs();
        }
    }

    private void InstantiatePrefabs()
    {
        PrefabsCreator.Get.LoadPrefab("Player/SceneInfo");
        
        var @params = new PrefabsCreator.PrefabParams {
            scale = new Vector3(1,1,1)
        };
        PrefabsCreator.Get.LoadPrefab("Player/Player", @params);
        PrefabsCreator.Get.LoadPrefab("Environment/TimeOfDay");
        
        
    }
    
}
