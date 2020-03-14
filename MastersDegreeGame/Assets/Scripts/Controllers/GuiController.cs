using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GuiController : MonoBehaviour
{
    public static GuiController Instance 
    { 
        get
        {
            if (_instance == null) {
                _instance = FindObjectOfType<GuiController>();
            }

            return _instance;
        }
    }
    private static GuiController _instance;

    public Canvas Canvas => _canvas;
    [SerializeField]
    private Canvas _canvas;

    public Camera Camera => _camera;
    [SerializeField]
    private Camera _camera;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Screen.fullScreen = true;
    }
    
    public void SetCameraClearFlags()
    {
        _camera.clearFlags = CameraClearFlags.Depth;
    }

    public void SetBlackScreen()
    {
        _camera.clearFlags = CameraClearFlags.SolidColor;
        _camera.backgroundColor = Color.black;
    }
}
