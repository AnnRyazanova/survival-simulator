using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private bool _started = false;

    public virtual void StartProperty(Object parent)
    {
        parentObject = parent;
        _started = true;
    }
    
    public virtual void AddPoints(int points) { }
    protected virtual void UpdateProperty() { }

    private void Update()
    {
        if (_started == false) return;
        UpdateProperty();
    }
}
