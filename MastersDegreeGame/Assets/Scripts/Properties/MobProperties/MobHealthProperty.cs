using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobHealthProperty : BaseProperty
{
    /// <summary>
    /// Максимальное кол-во здоровья
    /// </summary>
    [SerializeField] protected int _totalPoints;

    /// <summary>
    /// Текущий уровень здоровья
    /// </summary>
    protected int _currentPoints;

    public int CurrentPoints => _currentPoints;
    public float CurrentPointsNormalized => _currentPoints / (float)_totalPoints;
    public int TotalPoints => _totalPoints;
    
    public Action OnChange = () => { };

    public override void StartProperty(Object parent)
    {
        base.StartProperty(parent);
        
        _currentPoints = _totalPoints;
    }

    public override void AddPoints(int points)
    {
        base.AddPoints(points);
        _currentPoints = Math.Min(_totalPoints, _currentPoints + points);
        _currentPoints = Math.Max(0, _currentPoints);
        OnChange();
    }
}
