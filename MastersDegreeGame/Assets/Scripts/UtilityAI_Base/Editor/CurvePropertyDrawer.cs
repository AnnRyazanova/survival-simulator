using UnityEditor;
using UnityEngine;
using UtilityAI_Base.ResponseCurves;

namespace UtilityAI_Base.Editor
{
    [CustomPropertyDrawer(typeof(ResponseCurve))]
    public class CurvePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label) {
            
        }
    }
}