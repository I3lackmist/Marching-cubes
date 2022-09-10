using System;
using UnityEngine;

[Serializable]
public class DensityShaderProperties {
	[SerializeField]
	public int Octaves;

	[SerializeField]
	public float Lacunarity;

	[SerializeField]
	public float Persistence;

	[SerializeField]
	public float NoiseScale;

	[SerializeField]
	public float NoiseWeight;

	[SerializeField]
	public float FloorOffset;

	[SerializeField]
	public float WeightMultiplier;

	[SerializeField]
	public float HardFloor;

	[SerializeField]
	public float HardFloorWeight;

	[SerializeField]
	public bool CloseEdges;
}
