using UnityEngine;

public class NoiseMapTextureBaker : MonoBehaviour
{
	[SerializeField]
	public ComputeShader noiseMapShader;

	public static Texture2D BakeTexture(NoiseMapProperties properties) {

		int width = properties.width;
		int height = properties.height;

		float[] noiseArray = Noise.MakeNoiseArray(properties);

		Texture2D texture = new Texture2D(height, width);

		Color[] colorMap = new Color[width * height];

		int xChunks = width/8;
		int yChunks = height/8;

		for (int yChunk = 0; yChunk < yChunks; yChunk++) {
			for (int xChunk = 0; xChunk < xChunks; xChunk++) {
				for (int y = 0; y < 8; y++) {
					for (int x = 0; x < 8; x++) {
						int idx = yChunk * 8*width + y * width + xChunk * 8 + x;

						colorMap[idx] = Color.Lerp(Color.black, Color.white, noiseArray[idx]);
					}
				}
			}
		}

		texture.SetPixels(colorMap);
		texture.Apply();

		return texture;
	}

}
