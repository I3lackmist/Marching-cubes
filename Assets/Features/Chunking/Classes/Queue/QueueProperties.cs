using System;
using UnityEngine;

namespace MarchingCubes.Chunking.Classes 
{
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
}