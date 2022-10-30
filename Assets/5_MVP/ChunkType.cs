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

	public void SetShaderFields(ComputeShader shader) {
		shader.SetInt("octaves", octaves);
		shader.SetFloat("scale", scale);
		shader.SetFloat("lacunarity", lacunarity);
		shader.SetFloat("persistence", persistence);
	}
}