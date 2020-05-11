using UnityEngine;

public class DayNightControllerCreator
{
    private static DayNightControllerCreator _instance;
    public static DayNightControllerCreator Get => _instance ?? (_instance = new DayNightControllerCreator());
    
    private GameObject _gameObj;
    
    public void CreatePrefabOnScene()
    {
        if (_gameObj != null) {
            Debug.LogError("[DayNightCycleController::CreatePrefabOnScene] ERROR _gameObj is not empty");
            return;
        }
        
        _gameObj = null;
        var prefabPath = string.Format($"Environment/TimeOfDay");  
        var prefab = Resources.Load(prefabPath) as GameObject;
        if (prefab == null) {
            Debug.LogError("[DayNightCycleController::CreatePrefabOnScene] ERROR prefab is null");
            return;
        }
        
        _gameObj = GameObject.Instantiate(prefab) as GameObject;
        var vec = Vector3.zero;
        _gameObj.transform.localPosition = vec;
        _gameObj.transform.localScale = vec;
    }
}
