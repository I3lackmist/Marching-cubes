using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarchingCubes.Common.Interfaces;
using System.Linq;

namespace MarchingCubes.Chunking.Classes 
{
	public class DisposeQueue : MonoBehaviour, IQueue
	{
		[SerializeField]
		public QueueProperties properties;
		private List<IDisposable> disposeQueue;
		private List<IDisposable> disposedObjects;

		private int index;

        public QueueProperties QueueProperties { get => properties; }

        private void OnEnable() 
		{
			disposeQueue = new List<IDisposable>();
			disposedObjects = new List<IDisposable>();
		}

		public void Enqueue(IDisposable disposable) 
		{
			disposeQueue.Add(disposable);
		}

		public void Unqueue(IDisposable disposable) 
		{
			disposedObjects.Add(disposable);
		}

		private void CycleAction() 
		{
			var processThisCycle = (disposeQueue.Count > properties.processPerCycle) ? properties.processPerCycle : disposeQueue.Count;
			
			if (processThisCycle < 0 || processThisCycle > properties.processPerCycle) 
			{
				Debug.LogWarning($"Dispose queue processing less than 0 or more than max. Value: {processThisCycle}");
			}

			var disposables = disposeQueue.Take(processThisCycle);
		
			foreach (var disposable in disposables) {
				disposable.Dispose();
			}

			disposeQueue.RemoveRange(0, processThisCycle);
		}
		
		private IEnumerator Cycle() 
		{
			while (true) 
			{
				CycleAction();

				yield return new WaitForSeconds(properties.cycleLength);
			}
		}

		private void Start() 
		{
			if (!properties.processInUpdate) StartCoroutine(Cycle());
		}

		private void LateUpdate() 
		{
			if (properties.processInUpdate) CycleAction();
		}
	}
}