using System;
using System.Collections.Generic;
using UnityEngine;

namespace MarchingCubes.Chunking.Classes {

    [Serializable]
    public class Biome
    {
        [SerializeField]
        public List<DensityShaderPasses> densityShaderPasses;
        
        [SerializeField]
        public List<MarchingCubeShaderPass> marchingCubeShaderPasses;

        [SerializeField]
        public List<OverworldShaderPass> overworldShaderPasses;

    }
}
