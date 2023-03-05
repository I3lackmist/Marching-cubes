using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MarchingCubes.Common.Interfaces;

namespace MarchingCubes.Chunking.Classes 
{
	public class RenderQueue : MonoBehaviour, IQueue
	{
		[SerializeField]
		private QueueProperties properties;

		public Action Done;

		private int renderingCount = 0;
		private List<IRenderable> renderQueue;

		public QueueProperties QueueProperties => properties;

		public void Enqueue(IRenderable renderable) 
		{
			renderQueue.Add(renderable);
		}

		public void Unqueue(IRenderable renderable) 
		{
			renderQueue.Remove(renderable);
		}

		private void DoneRendering() 
		{
			renderingCount--;
			if (renderingCount < 0) {
				Debug.LogError("Rendering count is less than 0.");
			}
		}

		private void OnEnable() 
		{
			renderQueue = new List<IRenderable>();
			Done = new Action(DoneRendering);
		}

		private void Start() 
		{
			if (!properties.processInUpdate) StartCoroutine(Cycle());
		}

		private void Update() 
		{
			if (properties.processInUpdate) CycleAction();
		}

		private IEnumerator Cycle() 
		{
			while (true) {
				CycleAction();

				yield return new WaitForSeconds(properties.cycleLength);
			}
		}

		private void CycleAction() 
		{
			if (!renderQueue.Any()) return;

			var processThisCycle = properties.processPerCycle - renderingCount;
			processThisCycle = processThisCycle <= renderQueue.Count ? processThisCycle : renderQueue.Count; 

			var renderables = renderQueue.Take(processThisCycle);
		
			foreach (var renderable in renderables) {
				renderingCount++;
				renderable.Render(new Action(DoneRendering));
			}

			renderQueue.RemoveRange(0, processThisCycle);
		}
	}
}