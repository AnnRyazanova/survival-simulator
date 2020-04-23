using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthProperty : NeedProperty
{
    public override void StartProperty(Object parent)
    {
        base.StartProperty(parent);
#if  CHEAT
        //Debug.Log("Activate HealthProperty");
#endif
    }

    public override void AddPoints(int points)
    {
        base.AddPoints(points);
        if (parentObject.Type == Object.ObjectType.Player && _currentPoints <= 0) {
            DeathWindowController.Instance.ShowWindow();
        }
    }
}
