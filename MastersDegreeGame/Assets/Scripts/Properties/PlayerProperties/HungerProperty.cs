using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungerProperty : DecreaseNeedProperty
{
    public override void StartProperty(Object parent)
    {
        base.StartProperty(parent);
        Debug.Log("Activate HungerProperty");
    }
}
