using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Player;
using UnityEngine;
using Object = Characters.Object;

public class WarmProperty : DecreaseNeedProperty
{
    [SerializeField] private float _nightCoeff = 2;
    
    public override void StartProperty(Object parent)
    {
        base.StartProperty(parent);
#if  CHEAT
        //Debug.Log("Activate WarmProperty"); 
#endif
    }

    protected override void UpdateProperty()
    {
        if (CanUpdate() == false) return;
        
        if ((DateTime.Now - _lastUpdateTime).TotalSeconds >= _decreaseTime) {
            _lastUpdateTime = DateTime.Now;
            var coeff = DayNightCycleController.Get.CurrentTimeOfDay == DayNightCycleController.TimeOfDay.Night ||
                DayNightCycleController.Get.CurrentTimeOfDay == DayNightCycleController.TimeOfDay.Evening
                    ? _nightCoeff
                    : 1f;
            var value = Mathf.RoundToInt(_decreasePoints * coeff);
            //Debug.Log($"warm = {value}");
            AddPoints(value);

            if (_currentPoints < _criticalLowBorder) {
                (parentObject as PlayerObject).Health.AddPoints(_criticalLowHpPoints);
            }
        }
    }
}
