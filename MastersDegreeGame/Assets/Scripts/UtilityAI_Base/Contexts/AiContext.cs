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
    public abstract class AiContext : MonoBehaviour, IAiContext
    {
        public IAgent owner;
        public GameObject target;
        
        protected Dictionary<string, float> PropertyValues = new Dictionary<string, float>();

        public abstract object GetParameter(AiContextVariable param); 
        
        protected virtual void Awake() {
            owner = GetComponent<NpcMainScript>();
            target = null;
            // Fill();
        }

        #region REFLECTION
        
        public object this[string paramName]
        {
            get => GetType().GetProperties()
                .First(p => p.GetCustomAttribute(typeof(NpcContextVar)) != null && p.Name == paramName)
                .GetValue(this, null);
            set => GetType().GetProperties()
                .First(p => p.GetCustomAttribute(typeof(NpcContextVar)) != null && p.Name == paramName)
                .SetValue(this, value, null);
        }
        
        #endregion
    }
}