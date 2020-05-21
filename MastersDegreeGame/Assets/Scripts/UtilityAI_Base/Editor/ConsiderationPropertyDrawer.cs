using UnityEditor;
using UnityEngine;
using UtilityAI_Base.Considerations;


namespace UtilityAI_Base.Editor
{
    [CustomPropertyDrawer(typeof(Consideration))]
    public class ConsiderationPropertyDrawer : PropertyDrawer
    {
        private static readonly float VerticalSpacing = 2 * EditorGUIUtility.singleLineHeight;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var quarterW = EditorGUIUtility.currentViewWidth / 4;
            position.x += 10;

            var isEnabled = property.FindPropertyRelative("isEnabled").boolValue;

            property.FindPropertyRelative("isEnabled").boolValue = EditorGUI.Toggle(
                new Rect(position.x, position.y, 10, EditorGUIUtility.singleLineHeight),
                GUIContent.none, isEnabled);

            EditorGUI.BeginProperty(position, label, property);
            property.FindPropertyRelative("description").stringValue = EditorGUI.TextField(
                new Rect(position.x + 25, position.y, position.width - quarterW, EditorGUIUtility.singleLineHeight),
                property.FindPropertyRelative("description").stringValue);

            position.y += VerticalSpacing;

            EditorGUI.PropertyField(new Rect(position.x, position.y,
                    position.width - quarterW,
                    EditorGUIUtility.singleLineHeight), property.FindPropertyRelative("responseCurveType"),
                new GUIContent("Utility response curve"));
            
            EditorGUI.EndProperty();
        }
    }
}