﻿using System;
using Characters.Player;
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

    /// <summary>
    /// Единицы, ниже которых у персонажа начинает отниматься здоровье
    /// </summary>
    [SerializeField] protected int _criticalLowBorder;
    
    /// <summary>
    /// Единицы, здоровья, которые отнимаются при низких показателях
    /// </summary>
    [SerializeField] protected int _criticalLowHpPoints;

    public bool IsCritical => _currentPoints < _criticalLowBorder;
    
    protected override void UpdateProperty()
    {
        if (CanUpdate() == false) return;
        
        if ((DateTime.Now - _lastUpdateTime).TotalSeconds >= _decreaseTime) {
            _lastUpdateTime = DateTime.Now;
            AddPoints(_decreasePoints);

            if (_currentPoints < _criticalLowBorder) {
                (parentObject as PlayerObject).Health.AddPoints(_criticalLowHpPoints);
            }
        }
    }
}
