using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshSplit))]
public class MeshSplitEditor : Editor
{
    public override void OnInspectorGUI()
    {

        MeshSplit myScript = (MeshSplit)target;

        if (myScript.childen != null && myScript.childen.Count != 0)
        EditorGUILayout.HelpBox("Submesh count: " + myScript.childen.Count, MessageType.Info, true);
        else
        EditorGUILayout.HelpBox("Submesh count: none", MessageType.Info, true);

        DrawDefaultInspector();

        if (GUILayout.Button("Split"))
        {
            myScript.Split();
        }

        if (GUILayout.Button("Clear"))
        {
            myScript.Clear();
        }

    }
}