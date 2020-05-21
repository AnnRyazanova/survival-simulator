using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            var properties = GetType().GetProperties().Where(p => p.GetCustomAttribute(typeof(NpcContextVar)) != null);
            foreach (var property in properties) {
                var value = (float) property.GetValue(this);
                if (PropertyValues.ContainsKey(property.Name)) {
                    PropertyValues[property.Name] = value;
                }
                else {
                    PropertyValues.Add(property.Name, value);
                }
            }
        }
    }
}