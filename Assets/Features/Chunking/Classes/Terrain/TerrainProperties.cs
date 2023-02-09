using System;
using System.Collections.Generic;
using UnityEngine;
using MarchingCubes.Common.Enums;

namespace MarchingCubes.Chunking.Classes 
{
	[Serializable]
	public class TerrainProperties
	{
		[SerializeField]
		public GameObject chunkPrefab;

		[SerializeField]
		public List<ChunkType> chunkTypes;

		[SerializeField]
		public int chunkLimit;

		[SerializeField]
		public int brushSize;
	}
}