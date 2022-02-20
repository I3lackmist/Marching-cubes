using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChunkVolume))]
public class ChunkVolumeEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        ChunkVolume chunkVolume = (ChunkVolume)target;

        if (GUILayout.Button("Create terrain")) {
            chunkVolume.CreateTerrain();
        }

        if (GUILayout.Button("Reset")) {
            chunkVolume.Reset();
        }
    }
}
