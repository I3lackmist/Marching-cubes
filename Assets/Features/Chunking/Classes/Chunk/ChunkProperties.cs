using System;
using UnityEngine;

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