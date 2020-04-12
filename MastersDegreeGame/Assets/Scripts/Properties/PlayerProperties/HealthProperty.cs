using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthProperty : NeedProperty
{
    public override void StartProperty(Object parent)
    {
        base.StartProperty(parent);
        Debug.Log("Activate HealthProperty");
    }
}
