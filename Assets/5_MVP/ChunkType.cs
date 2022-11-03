using System;
using UnityEngine;

[Serializable]
public class ChunkType {

	[SerializeField]
	public float lacunarity;

	[SerializeField]
	public float persistence;

	[SerializeField]
	public int octaves;

	[SerializeField]
	public float scale;

	[SerializeField]
	public float maxValue;

	[SerializeField]
	public float isoLevel;

	public void SetShaderFields(ComputeShader noiseShader, ComputeShader cubeShader) {
		noiseShader.SetInt("octaves", octaves);
		noiseShader.SetFloat("scale", scale);
		noiseShader.SetFloat("lacunarity", lacunarity);
		noiseShader.SetFloat("persistence", persistence);
		noiseShader.SetFloat("maxValue", maxValue);

		cubeShader.SetFloat("isoLevel", isoLevel);
	}
}