using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthProperty : NeedProperty
{
    public override void StartProperty()
    {
        base.StartProperty();
        Debug.Log("Activate HealthProperty");
    }
}
