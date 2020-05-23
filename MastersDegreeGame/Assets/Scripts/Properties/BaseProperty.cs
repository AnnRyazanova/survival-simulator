using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = Characters.Object;

public class BaseProperty: MonoBehaviour
{
    /// <summary>
    /// Отложенный запуск работы свойства (для ресурсов)
    /// Пока ресурс не поднят, свойство не будет влиять на объект
    /// </summary>
    [SerializeField]
    private bool _isDeferred;
    public bool IsDeferred => _isDeferred;
    
    protected Object parentObject;
    protected bool _started = false;

    public virtual void StartProperty(Object parent)
    {
        parentObject = parent;
        _started = true;
    }
    
    public virtual void AddPoints(int points) { }
    public virtual void RemovePoints(int points) { }

    protected virtual void UpdateProperty() { }

    protected virtual bool CanUpdate()
    {
        return _started == true && GameSettingsController.Instance.IsPaused == false;
    }
    
    private void Update()
    {
        if (CanUpdate() == false) return;
        UpdateProperty();
    }
}
