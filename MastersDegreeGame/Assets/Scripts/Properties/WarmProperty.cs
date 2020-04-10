using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarmProperty : DecreaseNeedProperty
{
    public override void StartProperty()
    {
        base.StartProperty();
        Debug.Log("Activate WarmProperty"); 
    }
}
