using System;
using MarchingCubes.Common.Interfaces;
using UnityEngine;

namespace MarchingCubes.Chunking.Classes
{
    [Serializable]
    public class MarchingCubeShaderPass : IShaderPass
    {
        [SerializeField]
        public float isoLevel;

        [SerializeField]
        public float distanceBetweenPoints;

        public void SetProperties(ComputeShader shader)
        {
			shader.SetFloat("isoLevel", isoLevel);
            shader.SetFloat("distanceBetweenPoints", distanceBetweenPoints);
        }
    }
}
