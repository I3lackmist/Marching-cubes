using System.Collections.Generic;
using MarchingCubes.Common.Structs;
using UnityEngine;

namespace MarchingCubes.Common.Helpers {
	public static class MeshBaker
	{
		public static Mesh BakeMesh(ResultTriangle[] resultTris) {
				var mesh = new Mesh();

				List<Vector3> verts = new List<Vector3>();
				List<int> tris = new List<int>();

				int vertIndex = 0;

				foreach(ResultTriangle triangle in resultTris) {
					verts.Add(triangle.vertexA);
					verts.Add(triangle.vertexB);
					verts.Add(triangle.vertexC);

					tris.Add(vertIndex++);
					tris.Add(vertIndex++);
					tris.Add(vertIndex++);
				}

				mesh.vertices = verts.ToArray();
				mesh.triangles = tris.ToArray();

				mesh.RecalculateBounds();
				mesh.RecalculateNormals();

				return mesh;
		}
	}
}