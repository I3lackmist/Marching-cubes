using System;
using MarchingCubes.Common.Interfaces;
using UnityEngine;

namespace MarchingCubes.Chunking.Classes
{
    [Serializable]
    public class OverworldShaderPass : NoiseShaderPass, IShaderPass
    {
        [SerializeField]
        public float minHeight;
        
        [SerializeField]
        public float maxHeight;
        
        public override void SetProperties(ComputeShader shader)
        {
            base.SetProperties(shader);
            shader.SetFloat("minHeight", minHeight);
            shader.SetFloat("maxHeight", maxHeight);
        }
    }
}