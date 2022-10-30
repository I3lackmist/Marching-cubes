using System;
using UnityEngine;

[Serializable]
public class ChunkProperties
{
	[SerializeField]
	public Vector3Int chunkIndex;

	[SerializeField]
	public ChunkType chunkType;
}
