using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWindow : MonoBehaviour
{
    public virtual void Show() { }
    
    public virtual void Hide()
    {
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
        go = AddChild(GuiController.Instance.Canvas.gameObject, prefab);
        
        return go;
    }

    private static GameObject AddChild(GameObject parent, GameObject child)
    {
        GameObject go = GameObject.Instantiate(child) as GameObject;
        if (go != null && parent != null) {
            Transform t = go.transform;
            t.SetParent(parent.transform);
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
        }
        return go;
    }
    
    
}
