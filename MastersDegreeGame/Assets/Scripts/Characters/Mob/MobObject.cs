using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobObject : Object
{
    public MobHealthProperty Health { get; private set; }
    
    protected override void Start()
    {
        base.Start();
        Health = GetComponent<MobHealthProperty>();
    }
}
