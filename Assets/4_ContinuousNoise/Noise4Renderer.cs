using System;
using UnityEngine;

public class Noise4Renderer {
	const int num_threads = 8;

	private Noise4Properties _properties;
	private RenderTextureDescriptor _renderTextureDescriptor;
	private RenderTexture _renderTexture;
	public Noise4Renderer(Noise4Properties properties) {
		_renderTexture = new RenderTexture(
			properties.width,
			properties.height,
			32
		);
		_renderTexture.enableRandomWrite = true;

		_properties = properties;

		_renderTextureDescriptor = new RenderTextureDescriptor(
			_properties.width,
			_properties.height,
			RenderTextureFormat.ARGB32
		);
	}

    public void DrawNoiseTexture(NoiseMap4 noiseMap) {
		_properties.shader.SetTexture(0, "renderTexture", _renderTexture);

		_properties.shader.SetInt("seed", _properties.seed);
		_properties.shader.SetInt("height", _properties.height);
		_properties.shader.SetInt("width", _properties.width);
		_properties.shader.SetInt("octaves", _properties.octaves);
		_properties.shader.SetInt("chunkX", noiseMap.chunkIndex.x);
		_properties.shader.SetInt("chunkY", noiseMap.chunkIndex.y);
		_properties.shader.SetFloat("scale", _properties.scale);
		_properties.shader.SetFloat("lacunarity", _properties.lacunarity);
		_properties.shader.SetFloat("persistence", _properties.persistence);

		int xChunks = _properties.width / num_threads;
		int yChunks = _properties.height / num_threads;

		_properties.shader.Dispatch(0, xChunks, yChunks, 1);

		RenderTexture active = RenderTexture.active;
		RenderTexture.active = _renderTexture;

		noiseMap.texture.ReadPixels(
			new Rect(
				0,
				0,
				_renderTextureDescriptor.width,
				_renderTextureDescriptor.height
			),
			0,
			0
		);

		noiseMap.texture.Apply();
		noiseMap.SetOwnSprite();

		RenderTexture.active = active;
	}
}
