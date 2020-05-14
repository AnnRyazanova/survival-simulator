using UnityEditor;
using UnityEngine;
using UtilityAI_Base.Considerations;
using EditorGUILayout = UnityEditor.Experimental.Networking.PlayerConnection.EditorGUILayout;

namespace UtilityAI_Base.Editor
{
    [CustomPropertyDrawer(typeof(Consideration))]
    public class ConsiderationPropertyDrawer : PropertyDrawer
    {
        private readonly string[] _curveOptions = {"linear", "logistic", "quadratic"};

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var quarterW = EditorGUIUtility.currentViewWidth / 4;
            
            EditorGUI.BeginProperty(position, label, property);
            
            var isEnabled = property.FindPropertyRelative("isEnabled").boolValue;
            EditorGUI.LabelField(new Rect(position.x + 10, position.y, position.width, EditorGUIUtility.singleLineHeight), new GUIContent("Enabled"));
            
            property.FindPropertyRelative("isEnabled").boolValue = EditorGUI.Toggle(
                new Rect(position.x + 60, position.y, position.width - quarterW, EditorGUIUtility.singleLineHeight),
                GUIContent.none, isEnabled);

            EditorGUI.LabelField(new Rect(position.x + 10, position.y + EditorGUIUtility.singleLineHeight,position.width - 70f, 
                    EditorGUIUtility.singleLineHeight),
                new GUIContent("Response Curve"));

            property.FindPropertyRelative("curveId").intValue = EditorGUI.Popup(
                new Rect(position.x + quarterW, position.y + EditorGUIUtility.singleLineHeight,position.width - quarterW, 
                    EditorGUIUtility.singleLineHeight),
                property.FindPropertyRelative("curveId").intValue, _curveOptions);

                EditorGUI.EndProperty();
        }
    }
}