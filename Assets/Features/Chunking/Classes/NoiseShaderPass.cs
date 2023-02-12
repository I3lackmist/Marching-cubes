using MarchingCubes.Common.Enums;
using MarchingCubes.Common.Interfaces;
using UnityEngine;

namespace MarchingCubes.Chunking.Classes 
{
    public class NoiseShaderPass : IShaderPass
    {
        [SerializeField]
        public int octaves;
        
        [SerializeField]
        public float lacunarity;
        
        [SerializeField]
        public float persistence;
        
        [SerializeField]
        public float scale;
        
        [SerializeField]
        public float maxValue;
        
        [SerializeField]
        public int seed;

        [SerializeField]
        public int size;

        [SerializeField]
        public NoiseType noiseType;

        [SerializeField]
        public float constant;

        public virtual void SetProperties(ComputeShader shader)
        {
            shader.SetInt("noiseType", (int)noiseType);
            shader.SetInt("seed", seed);
            shader.SetInt("octaves", octaves);
            shader.SetFloat("scale", scale);
            shader.SetFloat("lacunarity", lacunarity);
            shader.SetFloat("persistence", persistence);
            shader.SetFloat("maxValue", maxValue);
            shader.SetFloat("size", size);
            shader.SetFloat("constant", constant);
        }
    }
}
