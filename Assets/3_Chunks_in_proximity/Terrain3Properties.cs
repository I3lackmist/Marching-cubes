using System;
using UnityEngine;

[Serializable]
public class Terrain3Properties
{
	[SerializeField]
	public Vector2Int chunkSize;

	[SerializeField]
	public GameObject chunkPrefab;

	[SerializeField]
	public int maxDistance;

	[SerializeField]
	public int minDistance;
}
