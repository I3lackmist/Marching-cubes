using System;
using System.Collections.Generic;
using UnityEngine;
using MarchingCubes.ShaderPasses.Interfaces;
using MarchingCubes.ShaderPasses.Classes;

namespace MarchingCubes.Biomes.MonoBehaviours 
{
    [Serializable]
    public class Biome : MonoBehaviour
    {
        [SerializeField]
        private List<HeightMapShaderPass> _heightMapShaderPasses;
        
        [SerializeField]
        private List<CaveGenShaderPass> _caveGenShaderPasses;
        
        [SerializeField]
        private List<MarchingCubeShaderPass> _marchingCubeShaderPasses;

        public List<IShaderPass> ShaderPasses()
        {
            var list = new List<IShaderPass>();
            
            list.AddRange(_heightMapShaderPasses);
            list.AddRange(_caveGenShaderPasses);
            list.AddRange(_marchingCubeShaderPasses);

            return list;
        }
    }
}
