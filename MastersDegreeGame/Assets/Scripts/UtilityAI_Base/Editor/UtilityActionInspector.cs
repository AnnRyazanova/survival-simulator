using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DefaultNamespace;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UtilityAI_Base.Actions;
using UtilityAI_Base.Considerations;
using UtilityAI_Base.CustomAttributes;
using UtilityAI_Base.Selectors;

namespace UtilityAI_Base.Editor
{
    [CustomEditor(typeof(UtilityAction))]
    public class UtilityActionInspector : UnityEditor.Editor
    {
        private ReorderableList _considerationsDisplay;
        private static readonly float VerticalSpacing = 2 * EditorGUIUtility.singleLineHeight;

        private UtilityAction SelectedAction => target as UtilityAction;

        private void SetConsiderationsListDrawCallback() {
            _considerationsDisplay.drawElementCallback = ConsiderationsListDrawCallback;
        }

        private void ConsiderationsListDrawCallback(Rect rect, int index, bool isactive, bool isfocused) {
            var consideration = _considerationsDisplay.serializedProperty.GetArrayElementAtIndex(index);
            var quarterW = EditorGUIUtility.currentViewWidth / 4;

            EditorGUI.PropertyField(rect, consideration);

            if (GUI.Button(new Rect(rect.width - quarterW / 2,
                rect.y + VerticalSpacing, 60, EditorGUIUtility.singleLineHeight), "Edit")) {
                CurveEditor.Open(SelectedAction.considerations[index].utilityCurve);
            }
        }

        private void OnEnable() {
            _considerationsDisplay = new ReorderableList(serializedObject,
                serializedObject.FindProperty("considerations"), true, true,
                true, true)
            {
                drawHeaderCallback = rect => { EditorGUI.LabelField(rect, "Considerations", EditorStyles.boldLabel); },
                elementHeight = EditorGUIUtility.singleLineHeight * 5 + 5,
                onAddCallback = AddItem,
                onRemoveCallback = RemoveItem
            };

            SetConsiderationsListDrawCallback();
            var selectors = typeof(ActionSelector).Assembly.GetTypes()
                .Where(type => type.IsClass && type.IsSubclassOf(typeof(ActionSelector)));
            foreach (var selector in selectors) {
                if (selector.GetCustomAttribute(typeof(ActionSelectorAttribute)) != null) {
                    Debug.Log(selector.GetFields().Length);
                    foreach (var memberInfo in selector.GetFields()) {
                        Debug.Log(memberInfo.Name);
                    }
                }
            }
        }

        private void AddItem(ReorderableList list) {
            SelectedAction.considerations.Add(new Consideration());
            EditorUtility.SetDirty(target);
        }

        private void RemoveItem(ReorderableList list) {
            SelectedAction.considerations.RemoveAt(list.index);
            EditorUtility.SetDirty(target);
        }

        private void ShowProperties() {
            EditorGUI.BeginChangeCheck();
            SelectedAction.qualifierTypeType =
                (QualifierType) UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Considerations Qualifier"),
                    SelectedAction.qualifierTypeType);

            if (EditorGUI.EndChangeCheck()) {
                // TODO: Possible GC issue. Revise later
                SelectedAction.qualifier =
                    ConsiderationsQualifierFactory.GetQualifier(SelectedAction.qualifierTypeType);
                Debug.Log(SelectedAction.qualifier);
            }

            EditorGUILayout.Separator();

            SelectedAction.CooldownTime = Mathf.Clamp(
                EditorGUILayout.FloatField(new GUIContent("Cooldown (ms)"), SelectedAction.CooldownTime),
                0f,
                100f);
            SelectedAction.ActionWeight =
                EditorGUILayout.FloatField(new GUIContent("Weight"), SelectedAction.ActionWeight);

            EditorGUILayout.Separator();
        }

        public void ShowDescription() {
            EditorGUILayout.LabelField(new GUIContent("Description"), EditorStyles.boldLabel);
            EditorGUILayout.Separator();
            SelectedAction.description = EditorGUILayout.TextArea(SelectedAction.description, GUILayout.Height(100));
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            ShowProperties();
            _considerationsDisplay.DoLayoutList();
            ShowDescription();
            serializedObject.ApplyModifiedProperties();

            // DrawDefaultInspector();
        }
    }
}