using UnityEditor;
using UnityEngine;
using UtilityAI_Base.Considerations;


namespace UtilityAI_Base.Editor
{
    [CustomPropertyDrawer(typeof(Consideration))]
    public class ConsiderationPropertyDrawer : PropertyDrawer
    {
        private static readonly float VerticalSpacing = 2 * EditorGUIUtility.singleLineHeight;

        public void DrawTitleLine(Rect position, SerializedProperty property, GUIContent label, float quarterW) {
            var tmpRect = new Rect(position.x + 25, position.y, position.width - 2 * quarterW,
                EditorGUIUtility.singleLineHeight);
            property.FindPropertyRelative("description").stringValue = EditorGUI.TextField(tmpRect,
                property.FindPropertyRelative("description").stringValue);
            
            tmpRect.x += position.width - 2 * quarterW + 15;
            tmpRect.width = quarterW / 3;
            
            EditorGUI.LabelField(tmpRect, "weight");
            tmpRect.x +=  quarterW / 3 + 5;
            
            property.FindPropertyRelative("weight").floatValue = EditorGUI.FloatField(tmpRect,
                property.FindPropertyRelative("weight").floatValue);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            var quarterW = EditorGUIUtility.currentViewWidth / 4;
            position.x += 10;
            var isEnabled = property.FindPropertyRelative("isEnabled").boolValue;

            property.FindPropertyRelative("isEnabled").boolValue = EditorGUI.Toggle(
                new Rect(position.x, position.y, 10, EditorGUIUtility.singleLineHeight),
                GUIContent.none, isEnabled);
            
            EditorGUI.BeginProperty(position, label, property);

            DrawTitleLine(position, property, label, quarterW);
           
            position.y += VerticalSpacing;

            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width - quarterW,
                    EditorGUIUtility.singleLineHeight), property.FindPropertyRelative("responseCurveType"),
                new GUIContent("Utility response curve"));
            position.y += VerticalSpacing;

            EditorGUI.EndProperty();
        }
    }
}