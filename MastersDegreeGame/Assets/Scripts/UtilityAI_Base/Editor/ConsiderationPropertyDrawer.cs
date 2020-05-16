using UnityEditor;
using UnityEngine;
using UtilityAI_Base.Considerations;
using UtilityAI_Base.ResponseCurves;
using EditorGUILayout = UnityEditor.Experimental.Networking.PlayerConnection.EditorGUILayout;

namespace UtilityAI_Base.Editor
{
    [CustomPropertyDrawer(typeof(Consideration))]
    public class ConsiderationPropertyDrawer : PropertyDrawer
    {
        private readonly string[] _curveOptions = {"linear", "logistic", "quadratic"};

        private static readonly float VerticalSpacing = 2 * EditorGUIUtility.singleLineHeight;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var quarterW = EditorGUIUtility.currentViewWidth / 4;
            var halfW = EditorGUIUtility.currentViewWidth / 2;
            position.x += 10;
            EditorGUI.BeginProperty(position, label, property);
            var isEnabled = property.FindPropertyRelative("isEnabled").boolValue;
            
            property.FindPropertyRelative("isEnabled").boolValue = EditorGUI.Toggle(
                new Rect(position.x , position.y, 10, EditorGUIUtility.singleLineHeight),
                GUIContent.none, isEnabled);
            
            property.FindPropertyRelative("description").stringValue = EditorGUI.TextField(
                new Rect(position.x + 25, position.y, position.width - quarterW, EditorGUIUtility.singleLineHeight),
                property.FindPropertyRelative("description").stringValue);

            position.y += VerticalSpacing;
            EditorGUI.LabelField(new Rect(position.x, position.y, position.width - 70f,
                    EditorGUIUtility.singleLineHeight),
                new GUIContent("Response Curve"));

            property.FindPropertyRelative("curveId").intValue = EditorGUI.Popup(
                new Rect(position.x + quarterW, position.y,
                    position.width - 2 * quarterW,
                    EditorGUIUtility.singleLineHeight),
                property.FindPropertyRelative("curveId").intValue, _curveOptions);
            
            EditorGUI.EndProperty();
        }
    }
}