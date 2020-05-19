using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UtilityAI_Base.Agents.Interfaces;
using UtilityAI_Base.Contexts.Interfaces;
using UtilityAI_Base.CustomAttributes;

namespace UtilityAI_Base.Contexts
{
    [Serializable]
    public class AiContext : MonoBehaviour, IAiContext
    {
        public IAgent owner;
        protected Dictionary<string, float> PropertyValues = new Dictionary<string, float>();
        
        public AiContext(IAgent owner) {
            this.owner = owner;
        }
        
        public float GetParameter(string paramName) {
            return PropertyValues[paramName];
        }
        
        public virtual void Fill() {
            foreach (var property in this.GetType().GetProperties()) {
                var f = 0f;
                try {
                    f = Convert.ToSingle(property.GetValue(this));
                }
                catch (Exception e) {
                }

                if (PropertyValues.ContainsKey(property.Name)) {
                    PropertyValues[property.Name] = f;
                }
                else {
                    PropertyValues.Add(property.Name, f);
                }
            }
        }
    }
}