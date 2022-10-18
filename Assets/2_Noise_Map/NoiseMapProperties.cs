using System;
using UnityEngine;

[Serializable]
public class NoiseMapProperties {

	[SerializeField]
	public int seed;

	[SerializeField]
	public int speed;
	public float time = 0;

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
