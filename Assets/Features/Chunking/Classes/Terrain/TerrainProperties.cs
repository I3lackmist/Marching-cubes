using System;
using System.Collections.Generic;
using UnityEngine;
using MarchingCubes.Common.Interfaces;

namespace MarchingCubes.Chunking.Classes 
{
	[Serializable]
	public class TerrainProperties
	{
		[SerializeField]
		public GameObject chunkPrefab;

		[SerializeField]
		public List<Biome> biomes;
		
		[SerializeField]
		public List<IShaderPass> shaderPasses;

		[SerializeField]
		public int chunkLimit;

		[SerializeField]
		public int brushSize;
	}
}