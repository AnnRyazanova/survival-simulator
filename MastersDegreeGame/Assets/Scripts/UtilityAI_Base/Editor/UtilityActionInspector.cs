using System;
using System.Reflection;
using Boo.Lang;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UtilityAI_Base.Actions;
using UtilityAI_Base.Considerations;
using UtilityAI_Base.CustomAttributes;
using UtilityAI_Base.ResponseCurves;
using UtilityAI_Base.ResponseCurves.SuppliedCurves;

namespace UtilityAI_Base.Editor
{
    [CustomEditor(typeof(UtilityAction))]
    public class UtilityActionInspector : UnityEditor.Editor
    {
        private ReorderableList _considerationsDisplay;
        private static readonly float VerticalSpacing = 2 * EditorGUIUtility.singleLineHeight;
        private readonly List<string> _qualifierOptions = new List<string>();
        private int _qualifierSelector = 0;

        private readonly ResponseCurve[] _curves =
            {new LinearResponseCurve(), new LogisticResponseCurve(), new QuadraticResponseCurve()};

        private UtilityAction SelectedAction => target as UtilityAction;

        private void SetConsiderationsListDrawCallback() {
            _considerationsDisplay.drawElementCallback = ConsiderationsListDrawCallback;
        }

        private void ConsiderationsListDrawCallback(Rect rect, int index, bool isactive, bool isfocused) {
            var consideration = _considerationsDisplay.serializedProperty.GetArrayElementAtIndex(index);
            var quarterW = EditorGUIUtility.currentViewWidth / 4;

            EditorGUI.PropertyField(rect, consideration);

            if (GUI.Button(new Rect(rect.width - quarterW / 2,
                rect.y + VerticalSpacing,
                60,
                EditorGUIUtility.singleLineHeight), "Edit")) {
                CurveEditor.Open(SelectedAction.considerations[index].utilityCurve,
                    SelectedAction.considerations[index].curveId);
            }

            (target as UtilityAction).considerations[index].utilityCurve =
                _curves[SelectedAction.considerations[index].curveId];
        }

        private void OnEnable() {
            _considerationsDisplay = new ReorderableList(serializedObject,
                serializedObject.FindProperty("considerations"), true, true,
                true, true)
            {
                drawHeaderCallback = rect => { EditorGUI.LabelField(rect, "Considerations"); },
                elementHeight = EditorGUIUtility.singleLineHeight * 5 + 5,
                onAddCallback = AddItem,
                onRemoveCallback = RemoveItem
            };

            SetConsiderationsListDrawCallback();
            FillConsiderationsOptions();
        }

        private void AddItem(ReorderableList list) {
            SelectedAction.considerations.Add(new Consideration());
            EditorUtility.SetDirty(target);
        }

        private void FillConsiderationsOptions() {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
                foreach (var type in assembly.GetTypes()) {
                    var attribute = type.GetCustomAttribute(typeof(ConsiderationsQualifierAttribute), false);
                    if (attribute is ConsiderationsQualifierAttribute c) {
                        _qualifierOptions.Add(c.Description);
                        SelectedAction.qualifier = SelectedAction.qualifier;
                    }
                }
            }
        }
        
        private void RemoveItem(ReorderableList list) {
            SelectedAction.considerations.RemoveAt(list.index);
            EditorUtility.SetDirty(target);
        }

        public override void OnInspectorGUI() {
            _qualifierSelector = EditorGUILayout.Popup("Qualifier", _qualifierSelector, _qualifierOptions.ToArray());
            serializedObject.Update();
            _considerationsDisplay.DoLayoutList();
            serializedObject.ApplyModifiedProperties();

            // DrawDefaultInspector();
        }
    }
}