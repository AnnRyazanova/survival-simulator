using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobObject : Object
{
    public HealthProperty Health { get; private set; }
    
    protected override void Start()
    {
        base.Start();
        Health = GetComponent<HealthProperty>();
        type = ObjectType.Mob;
    }
}
