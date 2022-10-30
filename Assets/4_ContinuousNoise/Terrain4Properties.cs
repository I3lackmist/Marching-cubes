using System;
using UnityEngine;

[Serializable]
public class Terrain4Properties
{
	[SerializeField]
	public Vector2 chunkSize;

	[SerializeField]
	public GameObject chunkPrefab;

	[SerializeField]
	public int maxDistance;

	[SerializeField]
	public int minDistance;
}
