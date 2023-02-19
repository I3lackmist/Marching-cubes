using UnityEngine;
using MarchingCubes.Common.Classes;

namespace MarchingCubes.Common.Helpers
{
    public static class NoisePropertiesExtensions
    {
        public static void SetProperties(this NoiseProperties noiseProperties, ComputeShader computeShader) 
        {
            computeShader.SetInt("noiseType", (int)noiseProperties.noiseType);
            computeShader.SetInt("seed", noiseProperties.seed);
            computeShader.SetInt("octaves", noiseProperties.octaves);
            computeShader.SetFloat("scale", noiseProperties.scale);
            computeShader.SetFloat("lacunarity", noiseProperties.lacunarity);
            computeShader.SetFloat("persistence", noiseProperties.persistence);
            computeShader.SetFloat("size", noiseProperties.size);
        }
    }
}