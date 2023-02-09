using UnityEngine;

public static class Noise {

    public static float[] MakeNoiseArray(NoiseMapProperties properties) {
		float[] noiseMap = new float[properties.width * properties.height];

		ComputeBuffer resultBuffer = new ComputeBuffer(properties.width * properties.height, sizeof(float));
		properties.shader.SetBuffer(0, "noiseData", resultBuffer);

		RenderTexture renderTexture = new RenderTexture(properties.width, properties.height, 24);
		properties.shader.SetTexture(0, "renderTexture", renderTexture);

		properties.shader.SetInt("height",properties.height);
		properties.shader.SetInt("width",properties.width);
		properties.shader.SetInt("octaves", properties.octaves);

		properties.shader.SetFloat("scale", properties.scale);
		properties.shader.SetFloat("lacunarity", properties.lacunarity);
		properties.shader.SetFloat("persistence", properties.persistence);

		int xChunks = properties.width/8;
		int yChunks = properties.height/8;

		properties.shader.Dispatch(0, xChunks, yChunks, 1);

		resultBuffer.GetData(noiseMap);
		resultBuffer.Release();
		renderTexture.Release();

		return noiseMap;
	}

    public static void MakeNoiseTexture(NoiseMapProperties properties, Texture2D targetTexture) {
		RenderTexture renderTexture = new RenderTexture(properties.width, properties.height, 24);
		renderTexture.enableRandomWrite = true;
		
		int kernelId = properties.shader.FindKernel("NoiseMap");
		properties.shader.SetTexture(kernelId, "renderTexture", renderTexture);

		properties.shader.SetInt("seed",properties.seed);

		properties.shader.SetInt("height",properties.height);
		properties.shader.SetInt("width",properties.width);
		properties.shader.SetInt("octaves", properties.octaves);

		properties.shader.SetFloat("time", properties.time);
		properties.shader.SetFloat("scale", properties.scale);
		properties.shader.SetFloat("lacunarity", properties.lacunarity);
		properties.shader.SetFloat("persistence", properties.persistence);

		int xChunks = properties.width / 8;
		int yChunks = properties.height / 8;

		properties.shader.Dispatch(kernelId, xChunks, yChunks, 1);

		RenderTexture.active = renderTexture;
		targetTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
		targetTexture.Apply();

		RenderTexture.active = default;

		renderTexture.Release();
	}

}
