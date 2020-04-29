using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    public class Object: MonoBehaviour
    {
        public enum ObjectType
        {
            Player,
            Mob,
        }

        public List<BaseProperty> Properties { get; private set; } = new List<BaseProperty>();
    
    
        protected ObjectType type;
        public ObjectType Type => type;

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
}
