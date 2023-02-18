using UnityEngine;
using UnityEditor;
using MarchingCubes.Biomes.MonoBehaviours;

namespace MarchingCubes.Biomes.Editors
{
    [CustomEditor(typeof(BiomeMap))]
    [CanEditMultipleObjects]
    public class BiomeMapEditor: Editor
    {
        public override void OnInspectorGUI()
        {
            base.DrawDefaultInspector();

            var biomeMap = (BiomeMap)target;

            if (GUILayout.Button("Reset")) 
            {
                biomeMap.Reset();
            }
        }
    }
}