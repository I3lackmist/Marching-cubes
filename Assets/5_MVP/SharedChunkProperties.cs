using System;
using UnityEngine;

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
	public Transform player;
}
