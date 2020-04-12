using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Object: MonoBehaviour
{
    public List<BaseProperty> Properties { get; private set; } = new List<BaseProperty>();

    protected virtual void Start()
    {
        GetProperties();
    }

    private void GetProperties()
    {
         var props = GetComponents<BaseProperty>();
         foreach (var prop in props) {
             if (prop.IsDeferred == false) {
                 prop.StartProperty(this);
                 Properties.Add(prop);
             }
         }
    }
}
