using UnityEngine;

public class PrefabsCreator
{
    private static PrefabsCreator _instance;
    public static PrefabsCreator Get => _instance ?? (_instance = new PrefabsCreator());
    
    public GameObject LoadPrefab(string prefabPath)
    {
        var prefab = Resources.Load(prefabPath) as GameObject;
        if (prefab == null) {
            Debug.LogError($"[PrefabsCreator::CreatePrefabOnScene] ERROR prefab is null, path : {prefabPath}");
            return null;
        }
        
        var _gameObj = GameObject.Instantiate(prefab) as GameObject;
        var vec = Vector3.zero;
        _gameObj.transform.localPosition = vec;
        _gameObj.transform.localScale = vec;

        return _gameObj;
    }
}
