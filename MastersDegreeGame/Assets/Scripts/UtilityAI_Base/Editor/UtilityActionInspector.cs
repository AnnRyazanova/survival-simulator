using System;
using Boo.Lang;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UtilityAI_Base.Actions;
using UtilityAI_Base.Considerations;
using UtilityAI_Base.ResponseCurves;
using UtilityAI_Base.ResponseCurves.SuppliedCurves;

namespace UtilityAI_Base.Editor
{
    [CustomEditor(typeof(UtilityAction))]
    public class UtilityActionInspector : UnityEditor.Editor
    {
        private ReorderableList _considerationsDisplay;

        private readonly ResponseCurve[] _curves =
            {new LinearResponseCurve(), new LogisticResponseCurve(), new QuadraticResponseCurve()};

        private UtilityAction SelectedAction => target as UtilityAction;

        private void SetConsiderationsListDrawCallback() {
            _considerationsDisplay.drawElementCallback = ConsiderationsListDrawCallback;
        }

        private void ConsiderationsListDrawCallback(Rect rect, int index, bool isactive, bool isfocused) {
            var consideration = _considerationsDisplay.serializedProperty.GetArrayElementAtIndex(index);
            var quarterW = EditorGUIUtility.currentViewWidth / 4;
 
            EditorGUI.BeginChangeCheck();
            
            EditorGUI.PropertyField(rect, consideration);
            
            if (GUI.Button(new Rect(rect.width - quarterW / 2,
                rect.y,
                60,
                EditorGUIUtility.singleLineHeight), "Edit")) {
                CurveEditor.Open( SelectedAction.considerations[index].utilityCurve);
            }
            (target as UtilityAction).considerations[index].utilityCurve =
                _curves[SelectedAction.considerations[index].curveId];
            if (EditorGUI.EndChangeCheck()) {

                Debug.Log("Result  " +  (target as UtilityAction).considerations[index].utilityCurve);
            }
        }

        private void OnEnable() {
            _considerationsDisplay = new ReorderableList(serializedObject,
                serializedObject.FindProperty("considerations"), true, true,
                true, true)
            {
                drawHeaderCallback = rect => { EditorGUI.LabelField(rect, "Considerations"); },
                elementHeight = EditorGUIUtility.singleLineHeight * 5 + 5,
            };

            SetConsiderationsListDrawCallback();
            
            _considerationsDisplay.onAddCallback += AddItem;
            _considerationsDisplay.onRemoveCallback += RemoveItem;
        }

        private void AddItem(ReorderableList list) {
            SelectedAction.considerations.Add(new Consideration());
            EditorUtility.SetDirty(target);
        }

        private void RemoveItem(ReorderableList list) {
            SelectedAction.considerations.RemoveAt(list.index);
            EditorUtility.SetDirty(target);
        }
        
        public override void OnInspectorGUI() {
            serializedObject.Update();

            _considerationsDisplay.DoLayoutList();
            serializedObject.ApplyModifiedProperties();

            // DrawDefaultInspector();
        }
    }
}