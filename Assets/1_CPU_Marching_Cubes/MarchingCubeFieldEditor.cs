using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(MarchingCubeField))]
public class MarchingCubeFieldEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        MarchingCubeField marchingCubeField = (MarchingCubeField)target;

        if (GUILayout.Button("Generate field")) {
            marchingCubeField.GenerateValues();
        }

        if (GUILayout.Button("Generate mesh")) {
            marchingCubeField.MarchingCubes(0);
        }
    }
}
