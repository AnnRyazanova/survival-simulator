using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageProperty : BaseProperty
{
   public override void StartProperty()
   {
      base.StartProperty();
      
      Debug.Log("Activate DamageProperty");
   }

   protected override void UpdateProperty()
   {
      base.UpdateProperty();
   }
}
