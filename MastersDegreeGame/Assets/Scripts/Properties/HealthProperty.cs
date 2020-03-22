using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthProperty : BaseProperty
{
    public override void StartProperty()
    {
        base.StartProperty();
        Debug.Log("Activate HealthProperty");
    }
    
    protected override void UpdateProperty()
    {
        base.UpdateProperty();
    }
}
