﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Object: MonoBehaviour
{
    [HideInInspector]
    public List<BaseProperty> properties = new List<BaseProperty>();

    private void Start()
    {
        GetProperties();
    }

    private void GetProperties()
    {
         var props = GetComponents<BaseProperty>();
         foreach (var prop in props) {
             if (prop.IsDeferred == false) {
                 prop.StartProperty();
                 properties.Add(prop);
             }
         }
    }
}