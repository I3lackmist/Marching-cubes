using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChunkVolume))]
public class ChunkVolumeEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        ChunkVolume chunkVolume = (ChunkVolume)target;

        if (GUILayout.Button("Make chunks")) {
            chunkVolume.MakeChunks();
        }

        if (GUILayout.Button("Make meshes")) {
            chunkVolume.MakeMeshes();
        }

        if (GUILayout.Button("Reset chunks")) {
            chunkVolume.ResetChunks();
        }

        if (GUILayout.Button("Reset meshes")) {
            chunkVolume.ResetMeshes();
        }
    }
}
