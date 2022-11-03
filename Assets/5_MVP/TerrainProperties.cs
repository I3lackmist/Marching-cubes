using System;
using System.Collections.Generic;
using UnityEngine;

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
