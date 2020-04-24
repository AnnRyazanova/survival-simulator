using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = Characters.Object;

public class EnergyProperty : NeedProperty
{
    public override void StartProperty(Object parent)
    {
        base.StartProperty(parent);
#if  CHEAT
        //Debug.Log("Activate EnergyProperty");
#endif
    }
}
