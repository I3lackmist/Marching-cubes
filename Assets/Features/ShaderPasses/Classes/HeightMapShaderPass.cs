using System;
using System.Linq;
using UnityEngine;
using MarchingCubes.Common.Classes;
using MarchingCubes.Common.Helpers;
using MarchingCubes.ShaderPasses.Enums;
using MarchingCubes.ShaderPasses.Interfaces;

namespace MarchingCubes.ShaderPasses.Classes
{
    [Serializable]
    public class HeightMapShaderPass : IShaderPass
    {
        [SerializeField]
        public NoiseProperties noiseProperties;

        [SerializeField]
        public float minHeight;
        
        [SerializeField]
        public float maxHeight;

        [SerializeField]
        private ComputeShader _shader;

        private static string[] _acceptedBufferNames = {
            BufferName.NoiseValues,
            BufferName.HeightValues
        };

        public void Execute()
        {
            this.SetProperties();
            _shader.Dispatch(0, 1, 1, 1);
        }

        public void SetBuffer(string bufferName, ComputeBuffer computeBuffer)
        {
            if (_acceptedBufferNames.Any(name => name.Equals(bufferName))) 
            {
                _shader.SetBuffer(0, bufferName, computeBuffer);
            }
        }

        public void SetPosition(Vector3Int position)
        {
            _shader.SetFloats(ShaderPropertyName.ChunkIndex, position.ToFloatArray());
        }

        private void SetProperties()
        {
            noiseProperties.SetProperties(_shader);

            _shader.SetFloat("minHeight", minHeight);
            _shader.SetFloat("maxHeight", maxHeight);
        }
    }
}