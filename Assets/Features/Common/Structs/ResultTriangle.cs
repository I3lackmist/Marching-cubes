using UnityEngine;

namespace MarchingCubes.Common.Structs {
	[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
	public struct ResultTriangle {
		public Vector3 vertexA;
		public Vector3 vertexB;
		public Vector3 vertexC;
	}
}