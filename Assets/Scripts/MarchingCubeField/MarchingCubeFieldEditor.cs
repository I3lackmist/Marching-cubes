using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(MarchingCubeField))]
public class MarchingCubeFieldEditor : Editor {
    
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        MarchingCubeField marchingCubeField = (MarchingCubeField)target;

        if (GUILayout.Button("Regenerate field")) {
            marchingCubeField.RegenerateValues();
        }
        if (GUILayout.Button("Generate mesh")) {
            marchingCubeField.GenerateMesh();
        }

        marchingCubeField.NumPointsAlongAxis = EditorGUILayout.IntSlider("Field size", marchingCubeField.NumPointsAlongAxis, 2, 20);
        marchingCubeField.DistanceBetweenPoints = EditorGUILayout.Slider("Distance between points", marchingCubeField.DistanceBetweenPoints,1,1000);
        marchingCubeField.FieldCenter = EditorGUILayout.Vector3Field("Field center", marchingCubeField.FieldCenter);
    }
}
