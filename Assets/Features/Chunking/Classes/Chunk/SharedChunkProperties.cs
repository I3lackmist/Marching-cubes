using System;
using UnityEngine;

namespace MarchingCubes.Chunking.Classes 
{
	[Serializable]
	public class SharedChunkProperties
	{
		[SerializeField]
		public float worldSize;

		[SerializeField]
		public float maxDistanceFromChunk;
		
		[SerializeField]
		public float distanceBetweenPoints;

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