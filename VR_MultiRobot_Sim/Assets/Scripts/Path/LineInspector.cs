using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LinearPath))]
public class LineInspector : Editor
{
    private void OnSceneGUI()
    {
        LinearPath line = target as LinearPath;

        Handles.color = Color.white;
        for(int i = 1; i < line.Paths[0].TTargets.Length; i++)
            Handles.DrawLine(line.Paths[0].TTargets[i-1].position, line.Paths[0].TTargets[i].position);
    }

    void OnRenderObject()
    {
        LinearPath line = target as LinearPath;

        Handles.color = Color.white;
        for (int i = 1; i < line.Paths[0].TTargets.Length; i++)
            Handles.DrawLine(line.Paths[0].TTargets[i - 1].position, line.Paths[0].TTargets[i].position);
    }
}
