using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NoiseMap))]
public class NoiseMapEditor : Editor {

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        NoiseMap noiseMap = (NoiseMap)target;

		if (GUILayout.Button("Make some noise")) {
            noiseMap.Draw();
        }

		if (GUILayout.Button("Reset")) {
			noiseMap.Reset();
		}

	}
}
