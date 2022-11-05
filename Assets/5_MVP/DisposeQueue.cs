using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisposeQueue : MonoBehaviour
{
	[SerializeField]
	public QueueProperties properties;
	private List<IDisposable> disposables;
	private List<IDisposable> disposedObjects;

	private int index;
	private void OnEnable() {
		disposables = new List<IDisposable>();
		disposedObjects = new List<IDisposable>();
	}

	public void Enqueue(IDisposable disposable) {
		disposables.Add(disposable);
	}

	public void Unqueue(IDisposable disposable) {
		disposedObjects.Add(disposable);
	}

	private void CycleAction() {
		foreach (var disposedObject in disposedObjects.ToArray()) {
			disposables.Remove(disposedObject);
			disposedObjects.Remove(disposedObject);
		}

		int processThisCycle = (disposables.Count > properties.processPerCycle) ? properties.processPerCycle : disposables.Count;

		for (int i = 0; i < processThisCycle; i++) {
			index = (index++) % disposables.Count;
			IDisposable disposable = disposables[index];
			if (!disposedObjects.Contains(disposable)) disposable.Dispose();
		}
	}
	private IEnumerator Cycle() {
		while (true) {
			CycleAction();

			yield return new WaitForSeconds(properties.cycleLength);
		}
	}
	private void Start() {
		if (!properties.processInUpdate) StartCoroutine(Cycle());
	}

	private void LateUpdate() {
		if (properties.processInUpdate) CycleAction();
	}
}
