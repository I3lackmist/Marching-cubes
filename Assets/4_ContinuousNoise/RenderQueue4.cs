using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RenderQueue4
{
	private List<NoiseMap4> renderQueue;
	private Noise4Renderer noiseRenderer;

	public RenderQueue4(Noise4Properties properties) {
		renderQueue = new List<NoiseMap4>();
		noiseRenderer = new Noise4Renderer(properties);
	}

	public void Enqueue(NoiseMap4 noiseMap) {
		renderQueue.Add(noiseMap);
	}

	public void Unqueue(NoiseMap4 noiseMap) {
		renderQueue.Remove(noiseMap);
	}

	public void Draw(NoiseMap4 noiseMap) {
		noiseRenderer.DrawNoiseTexture(noiseMap);
	}

    public void Update()
    {
		if (renderQueue.Any()) {
			Draw(renderQueue.First());
			renderQueue.RemoveAt(0);
		}
    }
}
