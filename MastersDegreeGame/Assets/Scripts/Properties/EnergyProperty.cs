using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyProperty : BaseProperty
{
    public override void StartProperty()
    {
        base.StartProperty();
        
        Debug.Log("Activate EnergyProperty");
    }
    
    protected override void UpdateProperty()
    {
        base.UpdateProperty();
    }
}
