using UnityEditor;
using UnityEngine;
using UtilityAI_Base.ResponseCurves;
using UtilityAI_Base.ResponseCurves.SuppliedCurves;

namespace UtilityAI_Base.Editor
{
    public class CurveEditor : EditorWindow
    {
        private ResponseCurve _currentCurve = new LogisticResponseCurve();

        [MenuItem("Window/CurveEditor")]
        public static void ShowWindow() {
            EditorWindow.GetWindow<CurveEditor>().Show();
        }

        private void OnGUI() {
            EditorGUILayout.BeginVertical();

            var rectOutline = new Rect(0, 0, position.width - 200, position.height);
            var rect = new Rect(0, 10, position.width - 200, position.height - 10);

            EditorGUI.DrawRect(rectOutline, Color.black);
            EditorGUI.DrawRect(rect, Color.black);

            const float minYAxes = 0;
            const float maxYAxes = 1;

            var step = 1 / position.width;

            var previousPosition = new Vector3(0, _currentCurve.CurveFunction(0), 0);
            for (var x = step; x < 1; x += step) {
                var pos = new Vector3(x, _currentCurve.CurveFunction(x), 0);
                var start = new Vector3(rect.xMin + previousPosition.x * rect.width,
                    rect.yMax - ((previousPosition.y - minYAxes) / (maxYAxes - minYAxes)) * rect.height, 0);
                var end = new Vector3(rect.xMin + pos.x * rect.width,
                    rect.yMax - ((pos.y - minYAxes) / (maxYAxes - minYAxes)) * rect.height, 0);

                Handles.DrawBezier(start, end, start, end, Color.green, null, 4);

                previousPosition = pos;
            }

            EditorGUI.LabelField(new Rect(position.width - 200, 10, position.width, position.height),
                new GUIContent("ssssssssssss"));

            EditorGUILayout.EndVertical();
        }
    }
}