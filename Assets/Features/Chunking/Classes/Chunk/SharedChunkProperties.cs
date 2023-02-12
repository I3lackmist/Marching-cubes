using System;
using UnityEngine;

namespace MarchingCubes.Chunking.Classes 
{
	[Serializable]
	public class SharedChunkProperties
	{
		[SerializeField]
		public int seed;

		[SerializeField]
		public float size;

		[SerializeField]
		public float worldSize;

		[SerializeField]
		public float maxDistanceFromChunk;

		[SerializeField]
		public ComputeShader marchingCubesShader;

		[SerializeField]
		public ComputeShader densityShader;
		[SerializeField]
		public ComputeShader overworldCutoffShader;

		[SerializeField]
		public bool alwaysVisible;

		[SerializeField]
		public Transform player;

		[SerializeField]
		public Transform parentTransform;

		[SerializeField]
		public Vector3Int playerGridPosition;
	}
}