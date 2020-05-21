using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DefaultNamespace;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UtilityAI_Base.Actions;
using UtilityAI_Base.Considerations;
using UtilityAI_Base.Contexts;
using UtilityAI_Base.CustomAttributes;
using UtilityAI_Base.ResponseCurves;
using UtilityAI_Base.ResponseCurves.SuppliedCurves;
using UtilityAI_Base.Selectors;

namespace UtilityAI_Base.Editor
{
    [CustomEditor(typeof(UtilityAction))]
    public class UtilityActionInspector : UnityEditor.Editor
    {
        private ReorderableList _considerationsDisplay;
        private static readonly float VerticalSpacing = 2 * EditorGUIUtility.singleLineHeight;
        private readonly List<string> _contextTypes = new List<string>();
        private readonly List<List<string>> _contexts = new List<List<string>>();
        private int _contextIndex = 0;
        private int _contextVarIndex = 0;

        private UtilityAction SelectedAction => target as UtilityAction;

        private void SetConsiderationsListDrawCallback() {
            _considerationsDisplay.drawElementCallback = ConsiderationsListDrawCallback;
        }

        private void ConsiderationsListDrawCallback(Rect rect, int index, bool isactive, bool isfocused) {
            var consideration = _considerationsDisplay.serializedProperty.GetArrayElementAtIndex(index);
            var quarterW = EditorGUIUtility.currentViewWidth / 4;

            EditorGUI.PropertyField(rect, consideration);
            if (SelectedAction.considerations[index].UtilityCurve is AnimationResponseCurve anim) {
                EditorGUI.CurveField(new Rect(rect.width - quarterW / 2,
                    rect.y + VerticalSpacing, 100, EditorGUIUtility.singleLineHeight), anim.curve);
            }else
            if (GUI.Button(new Rect(rect.width - quarterW / 2,
                rect.y + VerticalSpacing, 60, EditorGUIUtility.singleLineHeight), "Edit")) {
                CurveEditor.Open(SelectedAction.considerations[index].UtilityCurve);
            }

            SelectedAction.considerations[index].evaluatedContextVariableId = EditorGUI.Popup(
                new Rect(rect.x + 10, rect.y + 2 * VerticalSpacing, rect.width - quarterW,
                    EditorGUIUtility.singleLineHeight),
                "Target parameter",
                SelectedAction.considerations[index].evaluatedContextVariableId, _contexts[_contextIndex].ToArray());

            SelectedAction.considerations[index].evaluatedContextVariable =
                _contexts[_contextIndex][SelectedAction.considerations[index].evaluatedContextVariableId];
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
            FillCtxVariablesList();
        }

        public void FillCtxVariablesList() {
            var contexts = typeof(AiContext).Assembly.GetTypes()
                .Where(type => type.IsClass && type.IsSubclassOf(typeof(AiContext)));
            var ctxId = 0;
            foreach (var ctx in contexts) {
                _contexts.Add(new List<string>());
                _contextTypes.Add(ctx.Name);
                foreach (var memberInfo in ctx.GetProperties()) {
                    if (memberInfo.GetCustomAttribute(typeof(NpcContextVar)) != null) {
                        _contexts[ctxId].Add(memberInfo.Name);
                    }
                }

                ctxId++;
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
            SelectedAction.qualifierType =
                (QualifierType) UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Considerations Qualifier"),
                    SelectedAction.qualifierType);

            if (EditorGUI.EndChangeCheck()) {
                // TODO: Possible GC issue. Revise later
                SelectedAction.qualifier =
                    ConsiderationsQualifierFactory.GetQualifier(SelectedAction.qualifierType);
                Debug.Log(SelectedAction.qualifier);
            }

            EditorGUILayout.Separator();

            _contextIndex = EditorGUILayout.Popup(new GUIContent("Context"), _contextIndex, _contextTypes.ToArray());

            SelectedAction.CooldownTime = Mathf.Clamp(
                EditorGUILayout.FloatField(new GUIContent("Cooldown (ms)"), SelectedAction.CooldownTime),
                0f,
                100f);
            SelectedAction.ActionWeight =
                EditorGUILayout.FloatField(new GUIContent("Weight"), SelectedAction.ActionWeight);
            SelectedAction.maxConsecutiveInvocations = (int) EditorGUILayout.IntField(
                new GUIContent("Max Consecutive Invocations"), SelectedAction.maxConsecutiveInvocations);
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
        }
    }
}