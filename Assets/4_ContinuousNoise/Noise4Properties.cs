using System;
using UnityEngine;

[Serializable]
public class Noise4Properties {

	[SerializeField]
	public int seed;

	[SerializeField]
	public ComputeShader shader;

	[SerializeField]
	public int height;

	[SerializeField]
	public int width;

	[SerializeField]
	public float lacunarity;

	[SerializeField]
	public float persistence;

	[SerializeField]
	public int octaves;

	[SerializeField]
	public float scale;
}
