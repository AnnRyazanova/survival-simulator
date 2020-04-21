using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyProperty : NeedProperty
{
    public override void StartProperty(Object parent)
    {
        base.StartProperty(parent);
#if  CHEAT
        Debug.Log("Activate EnergyProperty");
#endif
    }
}
