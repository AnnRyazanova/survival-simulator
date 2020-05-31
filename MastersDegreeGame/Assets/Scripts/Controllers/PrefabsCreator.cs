using UnityEngine;

public class PrefabsCreator
{
    private static PrefabsCreator _instance;
    public static PrefabsCreator Get => _instance ?? (_instance = new PrefabsCreator());

    public class PrefabParams
    {
        public Vector3 scale = Vector3.one;
        public Vector3 position = Vector3.zero;
    }
    
    public GameObject LoadPrefab(string prefabPath, PrefabParams @params = null)
    {
        var prefab = Resources.Load(prefabPath) as GameObject;
        if (prefab == null) {
            Debug.LogError($"[PrefabsCreator::CreatePrefabOnScene] ERROR prefab is null, path : {prefabPath}");
            return null;
        }
        
        var _gameObj = GameObject.Instantiate(prefab) as GameObject;
        
        var vecPos =  @params?.position ?? Vector3.zero;
        var vecScale = @params?.scale ??Vector3.one;
        _gameObj.transform.localPosition = vecPos;
        _gameObj.transform.localScale = vecScale;

        return _gameObj;
    }
}
