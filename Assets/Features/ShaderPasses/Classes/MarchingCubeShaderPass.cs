using System;
using System.Linq;
using UnityEngine;
using MarchingCubes.Common.Helpers;
using MarchingCubes.ShaderPasses.Enums;
using MarchingCubes.ShaderPasses.Interfaces;

namespace MarchingCubes.ShaderPasses.Classes
{
    [Serializable]
    public class MarchingCubeShaderPass : IShaderPass
    {
        [SerializeField]
        public float isoLevel;

        [SerializeField]
        private ComputeShader _shader;

        private static string[] _acceptedBufferNames = {
            BufferName.NoiseValues,
            BufferName.ResultTriangles
        };

        private static string[] _acceptedPropertyNames = {
            ShaderPropertyName.ChunkIndex,
            ShaderPropertyName.DistanceBetweenPoints
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

        public void SetProperty(string propertyName, float value)
        {
            if (_acceptedPropertyNames.Any(name => name.Equals(propertyName))) 
            {
                _shader.SetFloat(propertyName, value);
            }
        }

        public void SetProperty(string propertyName, Vector3Int value)
        {
            if (_acceptedPropertyNames.Any(name => name.Equals(propertyName))) 
            {
                _shader.SetFloats(propertyName, value.ToFloatArray());
            }
        }
       
        public void SetProperty(string propertyName, int value)
        {
            if (_acceptedPropertyNames.Any(name => name.Equals(propertyName))) 
            {
                _shader.SetFloats(propertyName, value);
            }
        }

        private void SetProperties()
        {
			_shader.SetFloat("isoLevel", isoLevel);
        }
    }
}

