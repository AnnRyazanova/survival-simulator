using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageProperty : BaseProperty
{
   public override void StartProperty(Object parent)
   {
      base.StartProperty(parent);
      
      Debug.Log("Activate DamageProperty");
   }

   protected override void UpdateProperty()
   {
      base.UpdateProperty();
   }
}
