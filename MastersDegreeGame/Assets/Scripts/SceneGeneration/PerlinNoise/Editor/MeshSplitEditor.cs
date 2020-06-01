using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshSplit))]
public class MeshSplitEditor : Editor
{
    public override void OnInspectorGUI()
    {

        MeshSplit myScript = (MeshSplit)target;

        if (myScript.children != null && myScript.children.Count != 0)
        EditorGUILayout.HelpBox("Submesh count: " + myScript.children.Count, MessageType.Info, true);
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