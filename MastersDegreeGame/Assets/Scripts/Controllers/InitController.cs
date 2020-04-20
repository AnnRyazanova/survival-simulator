using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitController : MonoBehaviour
{
    public static InitController Instance => _instance;
    private static InitController _instance = null;

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        GuiController.Instance.SetCameraClearFlags();
        MenuController.Instance.ShowWindow();
        SceneLoaderController.Instance.LoadMenuScene();
    }
}
