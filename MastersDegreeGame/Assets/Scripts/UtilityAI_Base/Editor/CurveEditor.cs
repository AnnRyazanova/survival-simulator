using System;
using UnityEditor;
using UnityEngine;
using UtilityAI_Base.ResponseCurves;
using UtilityAI_Base.ResponseCurves.SuppliedCurves;

namespace UtilityAI_Base.Editor
{
    public class CurveEditor : EditorWindow
    {
        private static ResponseCurve _currentCurve;
        private static int _curveId;

        private readonly ResponseCurve[] _defualtCurves =
            {new LinearResponseCurve(), new LogisticResponseCurve(), new QuadraticResponseCurve()};

        public static void ShowWindow() {
            EditorWindow.GetWindow<CurveEditor>().Show();
        }

        public static void Open(ResponseCurve curve, int curveId) {
            _currentCurve = curve;
            _curveId = curveId;
            ShowWindow();
        }

        private void OnGUI() {
            var horizontalSplit = position.width - position.width / 4f;
            var rectOutline = new Rect(0, 0, horizontalSplit, position.height);
            var rect = new Rect(10, 10, horizontalSplit - 20, position.height - 20);

            EditorGUILayout.BeginHorizontal();

            EditorGUI.DrawRect(rectOutline, Color.black);
            EditorGUI.DrawRect(rect, Color.black);

            if (_currentCurve != null) {
                DrawCurve(rect);
                var lhsRect = new Rect(horizontalSplit + 10, 10, position.width / 4f - 20,
                    EditorGUIUtility.singleLineHeight);

                EditorGUI.BeginChangeCheck();
                DrawLeftSideSliders(lhsRect);
                EditorGUI.EndChangeCheck();
            }

            EditorGUILayout.EndHorizontal();
        }

        public void DrawLeftSideSliders(Rect lhsRect) {
            EditorGUI.LabelField(lhsRect, new GUIContent("Slope"));
            lhsRect.y += EditorGUIUtility.singleLineHeight;
            _currentCurve.slope = EditorGUI.Slider(lhsRect, _currentCurve.slope, 0f, 100f);
            lhsRect.y += 2 * EditorGUIUtility.singleLineHeight;

            EditorGUI.LabelField(lhsRect, new GUIContent("Exponent"));
            lhsRect.y += EditorGUIUtility.singleLineHeight;

            _currentCurve.exponent = EditorGUI.Slider(lhsRect, _currentCurve.exponent, 0f, 100f);

            lhsRect.y += 2 * EditorGUIUtility.singleLineHeight;

            EditorGUI.LabelField(lhsRect, new GUIContent("HShift"));
            lhsRect.y += EditorGUIUtility.singleLineHeight;

            _currentCurve.horizontalShift = EditorGUI.Slider(lhsRect, _currentCurve.horizontalShift,
                -1f, 1f);

            lhsRect.y += 2 * EditorGUIUtility.singleLineHeight;

            EditorGUI.LabelField(lhsRect, new GUIContent("VShift"));
            lhsRect.y += EditorGUIUtility.singleLineHeight;

            _currentCurve.verticalShift = EditorGUI.Slider(lhsRect, _currentCurve.verticalShift,
                -1f, 1f);
            lhsRect.y += 2 * EditorGUIUtility.singleLineHeight;

            if (GUI.Button(lhsRect, "default")) {
                _currentCurve = _defualtCurves[_curveId];
            }
        }

        public void DrawCurve(Rect rect) {
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
        }
    }
}