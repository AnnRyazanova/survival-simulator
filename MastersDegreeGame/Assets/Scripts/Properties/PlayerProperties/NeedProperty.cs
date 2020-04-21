using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedProperty : BaseProperty
{
    /// <summary>
    /// Максимальное кол-во поинтов свойства
    /// </summary>
    [SerializeField] protected int _totalPoints;

    /// <summary>
    /// Время на восстановление 1 единицы свойства
    /// </summary>
    [SerializeField] protected int _recoveryTime;

    /// <summary>
    /// Единица свойства для восстановления в единицу времени
    /// </summary>
    [SerializeField] protected int _recoveryPoints;

    /// <summary>
    /// Не восстанавливать данное свойство от времени (для врагов)
    /// </summary>
    [SerializeField] protected bool _skipRecovery;
    
    /// <summary>
    /// Текущий уровень поинтов свойства
    /// </summary>
    protected int _currentPoints;

    /// <summary>
    /// Последнее время обновления поинтов свойства
    /// </summary>
    protected DateTime _lastUpdateTime;

    protected int _previousPoints;

    public int CurrentPoints => _currentPoints;
    public float CurrentPointsNormalized => _currentPoints / (float)_totalPoints;
    public int TotalPoints => _totalPoints;
    
    public Action OnChange = () => { };

    public override void StartProperty(Object parent)
    {
        base.StartProperty(parent);
        
        // TO DO: Если будет сохранение свойств, нужно будет изменить это
        _currentPoints = _totalPoints;
        _lastUpdateTime = DateTime.Now;
    }

    public override void AddPoints(int points)
    {
        base.AddPoints(points);
        _currentPoints = Math.Min(_totalPoints, _currentPoints + points);
        _currentPoints = Math.Max(0, _currentPoints);
        OnChange();
    }
    
    protected override void UpdateProperty()
    {
        base.UpdateProperty();
        
        Recovery();
    }

    protected virtual void Recovery()
    {
        if (_skipRecovery) return;
        
        // Восстановление свойства
        if ((DateTime.Now - _lastUpdateTime).TotalSeconds >= _recoveryTime && _currentPoints != _previousPoints) {
            _lastUpdateTime = DateTime.Now;
            _previousPoints = _currentPoints;
            AddPoints(_recoveryPoints);
        }
    }
}
