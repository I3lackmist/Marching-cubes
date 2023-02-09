using UnityEngine;

public class NoiseMap : MonoBehaviour {
	[SerializeField]
	public NoiseMapProperties noiseMapProperties;

	[SerializeField]
	Texture2D texture;

	void OnEnable() {
		texture = new Texture2D(noiseMapProperties.width, noiseMapProperties.height, TextureFormat.RGB24, false);
		gameObject.GetComponent<Renderer>().sharedMaterial.mainTexture = texture;
	}

	public void Reset() {
		if (texture != null) {
			if (Application.isPlaying) {
				Destroy(texture);
			}
			else {
				DestroyImmediate(texture);
			}

			texture = new Texture2D(noiseMapProperties.width, noiseMapProperties.height, TextureFormat.RGB24, false);

			gameObject.GetComponent<Renderer>().sharedMaterial.mainTexture = texture;
		}

		noiseMapProperties.time = 0;
	}

	public void Update() {
		if (noiseMapProperties.animate) {
			noiseMapProperties.time += Time.deltaTime * noiseMapProperties.speed;
			
			Draw();
		}
	}

	public void Draw() {
		if (texture == null) {
			texture = new Texture2D(noiseMapProperties.width, noiseMapProperties.height, TextureFormat.RGB24, false);
		}

		Noise.MakeNoiseTexture(noiseMapProperties, texture);

		gameObject.GetComponent<Renderer>().sharedMaterial.mainTexture = texture;

		noiseMapProperties.time = Mathf.PingPong(Time.time*noiseMapProperties.speed, 600f);
	}
}
