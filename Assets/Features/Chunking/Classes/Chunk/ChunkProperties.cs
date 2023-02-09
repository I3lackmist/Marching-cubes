using System;
using UnityEngine;
using MarchingCubes.Common.Enums;

namespace MarchingCubes.Chunking.Classes 
{
	[Serializable]
	public class ChunkProperties
	{
		[SerializeField]
		public Vector3Int chunkIndex;

		[SerializeField]
		public ChunkType chunkType;
	}
}