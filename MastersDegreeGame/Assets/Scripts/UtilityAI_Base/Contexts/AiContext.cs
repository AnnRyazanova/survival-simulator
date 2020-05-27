using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Characters.NPC;
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
        public GameObject target;

        protected Dictionary<string, float> PropertyValues = new Dictionary<string, float>();

        protected virtual void Awake() {
            owner = GetComponent<NpcMainScript>();
            target = null;
            Fill();
        }

        public float GetParameter(string paramName) {
            return PropertyValues[paramName];
        }

        public IEnumerable<T> GetSequenceParameter<T>(string paramName) {
            return new List<T>();
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