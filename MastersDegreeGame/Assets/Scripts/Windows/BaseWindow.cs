using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWindow : MonoBehaviour
{
    public Action OnWindowShow = delegate { };
    public Action OnWindowHide = delegate { };

    public virtual void Show()
    {
        OnWindowShow();
        Time.timeScale = 0;
    }
    
    public virtual void Hide()
    {
        OnWindowHide();
        Time.timeScale = 1;
        Destroy(gameObject);
    }
    
    public static BaseWindow LoadWindow(string prefabName)
    {
        var go = LoadPrefab(prefabName);
        if (go != null) {
            return go.GetComponent<BaseWindow>();
        }

        return null;
    }

    private static GameObject LoadPrefab(string prefabName)
    {
        GameObject go = null;
        var prefabPath = string.Format($"UI/{prefabName}");  
        var prefab = Resources.Load(prefabPath) as GameObject;
        if (prefab == null) {
            return null;
        }
        
        go = GameObject.Instantiate(prefab, GuiController.Instance.Canvas.transform) as GameObject;
        
        return go;
    }   
}
