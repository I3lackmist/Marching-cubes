using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RenderQueue : MonoBehaviour
{
	[SerializeField]
	public int renderPerCycle;

	public Action DoneFunction;

	public int renderingCount = 0;

	public List<IRenderable> renderQueue;

	public void Enqueue(IRenderable renderable) {
		renderQueue.Add(renderable);
	}

	private void DoneRendering() {
		renderingCount--;
	}

	private void OnEnable() {
		renderQueue = new List<IRenderable>();
		DoneFunction = new Action(DoneRendering);
	}

	private void FixedUpdate() {
		if (renderQueue.Any()) {
			renderQueue.Take(renderPerCycle - renderingCount).Select( renderable => {
				renderable.Render(DoneFunction);
				renderingCount++;
				renderQueue.RemoveAt(0);

				return renderable;
			});
		}
	}
}
