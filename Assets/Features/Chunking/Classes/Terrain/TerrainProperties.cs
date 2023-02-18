using System;
using System.Collections.Generic;
using UnityEngine;
using MarchingCubes.Biomes.MonoBehaviours;

namespace MarchingCubes.Chunking.Classes 
{
	[Serializable]
	public class TerrainProperties
	{
		[SerializeField]
		public GameObject chunkPrefab;

		[SerializeField]
		public BiomeMap biomeMap;

		[SerializeField]
		public int chunkLimit;

		[SerializeField]
		public int brushSize;
	}
}