using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageProperty : BaseProperty
{
   public override void StartProperty(Object parent)
   {
      base.StartProperty(parent);
#if  CHEAT
      //Debug.Log("Activate DamageProperty");
#endif
   }

   protected override void UpdateProperty()
   {
      base.UpdateProperty();
   }
}
