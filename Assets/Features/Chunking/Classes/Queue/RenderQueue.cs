using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MarchingCubes.Chunking.Interfaces;

namespace MarchingCubes.Chunking.Classes 
{
	public class RenderQueue : MonoBehaviour, IQueue
	{
		[SerializeField]
		private QueueProperties properties;
		public Action DoneFunction;

		private int renderingCount = 0;
		private List<IRenderable> renderQueue;

		public QueueProperties QueueProperties => properties;

		public void Enqueue(IRenderable renderable) {
			renderQueue.Add(renderable);
		}

		public void Unqueue(IRenderable renderable) {
			renderQueue.Remove(renderable);
		}

		private void DoneRendering() {
			renderingCount--;
		}

		private void OnEnable() {
			renderQueue = new List<IRenderable>();
			DoneFunction = new Action(DoneRendering);
		}

		private void Start() {
			if (!properties.processInUpdate) StartCoroutine(Cycle());
		}

		private void Update() {
			if (properties.processInUpdate) CycleAction();
		}

		private IEnumerator Cycle() {
			while (true) {
				CycleAction();

				yield return new WaitForSeconds(properties.cycleLength);
			}
		}

		private void CycleAction() {
			for (int i = 0; i < properties.processPerCycle - renderingCount; i++) {
					if (!renderQueue.Any()) break;

					renderQueue[0].Render(DoneFunction);
					renderQueue.RemoveAt(0);
					renderingCount++;
				}
		}
	}
}