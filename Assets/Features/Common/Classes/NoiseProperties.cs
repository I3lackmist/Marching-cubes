using System;
using UnityEngine;

using MarchingCubes.Common.Enums;

namespace MarchingCubes.Common.Classes
{
    [Serializable]
    public class NoiseProperties 
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
        public int seed;

        [SerializeField]
        public int size;

        [SerializeField]
        public float baseConstant;

        [SerializeField]
        public NoiseType noiseType;
    }
}