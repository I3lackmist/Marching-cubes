using System;
using UnityEngine;
using MarchingCubes.Biomes.MonoBehaviours;

namespace MarchingCubes.Chunking.Classes 
{
	[Serializable]
	public class ChunkProperties
	{
		[SerializeField]
		public Vector3Int chunkIndex;

		[SerializeField]
		public Biome biome;
	}
}