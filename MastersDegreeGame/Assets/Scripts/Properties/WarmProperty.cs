using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarmProperty : BaseProperty
{
    public override void StartProperty()
    {
        base.StartProperty();
        
        Debug.Log("Activate WarmProperty");
    }
    
    protected override void UpdateProperty()
    {
        base.UpdateProperty();
    }
}
