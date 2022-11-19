using System;
using System.Collections.Generic;
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

	[SerializeField]
	public List<ComputeShader> filterShaders;

	public void SetShaderFields(ComputeShader shader) {
		shader.SetInt("octaves", octaves);
		shader.SetFloat("scale", scale);
		shader.SetFloat("lacunarity", lacunarity);
		shader.SetFloat("persistence", persistence);
		shader.SetFloat("maxValue", maxValue);
		shader.SetFloat("isoLevel", isoLevel);
	}

	public void SetShaderFields(ComputeShader noiseShader, ComputeShader cubeShader) {
		SetShaderFields(noiseShader);

		cubeShader.SetFloat("isoLevel", isoLevel);
	}
}