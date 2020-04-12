using System;
using UnityEngine;

public class DecreaseNeedProperty : NeedProperty
{
    /// <summary>
    /// Время, через которое уменьшается показатель
    /// </summary>
    [SerializeField] protected int _decreaseTime;

    /// <summary>
    /// Единица, на которую уменьшается показатель в единицу времени
    /// </summary>
    [SerializeField] protected int _decreasePoints;
    
    protected override void UpdateProperty()
    {
        if ((DateTime.Now - _lastUpdateTime).TotalSeconds >= _decreaseTime && _currentPoints != _previousPoints) {
            _lastUpdateTime = DateTime.Now;
            _previousPoints = _currentPoints;
            AddPoints(_decreasePoints);
        }
    }
}
