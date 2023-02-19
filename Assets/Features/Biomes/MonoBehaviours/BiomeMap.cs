using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MarchingCubes.Biomes.MonoBehaviours 
{
    public class BiomeMap : MonoBehaviour
    {
        private List<Biome> _biomes = new List<Biome>();
        
        [ReadOnly]
        public int biomeCount = 0;

        public void Reset()
        {
            _biomes.Clear();
            
            _biomes.AddRange(gameObject.GetComponentsInChildren<Biome>());

            biomeCount = _biomes.Count;
        }

        public Biome GetRandomBiome()
        {
            return _biomes[Random.Range(0, biomeCount-1)];
        }

        public Biome GetBiome(string biomeName) 
        {
            return _biomes.First(biome => gameObject.name == biomeName);
        }

        public Biome GetBiome(Vector3Int position) 
        {
            var noise = Mathf.PerlinNoise(position.x, position.z);
            var biomeIndex = Mathf.RoundToInt(noise * biomeCount);

            return _biomes[biomeIndex] ?? _biomes[0];
        }

        private void Start() 
        {
            Reset();

            if (biomeCount < 1) 
            {
                Debug.LogError("No biomes set up in BiomeMap!");
            }
        }
    }
}