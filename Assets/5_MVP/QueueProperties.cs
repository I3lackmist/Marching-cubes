using System;
using UnityEngine;

[Serializable]
public class QueueProperties
{
	[SerializeField]
	public int processPerCycle;

	[SerializeField]
	public float cycleLength;

	[SerializeField]
	public bool processInUpdate;
}
