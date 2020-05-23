using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWindow : MonoBehaviour
{
    public enum WindowId
    {
        Menu = 0,
        MainWindow = 1,
        Inventory = 2,
        Craft = 3,
        DeathWindow = 4, 
        Settings = 5,
    }
    
    public Action OnWindowShow = delegate { };
    public Action OnWindowHide = delegate { };
    public WindowId id;
    
    public virtual void Show()
    {
        OnWindowShow();
        OnShow();
    }
    
    public virtual void Hide()
    {
        OnWindowHide();
        OnHide();
        Destroy(gameObject);
    }

    protected virtual void OnShow()
    {
        if (id != WindowId.Menu) {
            GameSettingsController.Instance.IsPaused = true;
        }
    }

    protected virtual void OnHide()
    {
        if (id != WindowId.Menu) {
            GameSettingsController.Instance.IsPaused = false;
        }
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
