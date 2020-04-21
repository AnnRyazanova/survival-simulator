using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarmProperty : DecreaseNeedProperty
{
    public override void StartProperty(Object parent)
    {
        base.StartProperty(parent);
#if  CHEAT
        Debug.Log("Activate WarmProperty"); 
#endif
    }
}
