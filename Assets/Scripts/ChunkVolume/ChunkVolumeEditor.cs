using System.Collections;
using System.Collections.Generic;
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
    }
}
