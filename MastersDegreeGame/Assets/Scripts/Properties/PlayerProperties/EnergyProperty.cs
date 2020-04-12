using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyProperty : NeedProperty
{
    public override void StartProperty()
    {
        base.StartProperty();
        Debug.Log("Activate EnergyProperty");
    }
}
