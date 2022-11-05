using System;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour, IRenderable, IDisposable
{
	public Terrain parent;
	private Mesh mesh;
	public SharedChunkProperties sharedProperties;
	public ChunkProperties properties;

	private MeshFilter meshFilter;

	public void MakeMesh(ResultTriangle[] resultTris) {
		mesh = new Mesh();

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

		meshFilter.sharedMesh = mesh;
	}

	public void Render(Action doneFunction) {
		SetShaderValues();
		DispatchShaders();

		doneFunction();

		parent.EnqueueDispose(properties.chunkIndex);
	}

	private void SetShaderValues() {
		sharedProperties.densityShader.SetInt("seed", sharedProperties.seed);

		properties.chunkType.SetShaderFields(sharedProperties.densityShader, sharedProperties.marchingCubesShader);

		sharedProperties.densityShader.SetInts(
			"chunkIndex",
			new int[] {
				properties.chunkIndex.x,
				properties.chunkIndex.y,
				properties.chunkIndex.z
			}
		);

		sharedProperties.marchingCubesShader.SetFloat("distanceBetweenPoints", transform.lossyScale.x/8);
	}

	private void DispatchShaders() {
		ComputeBuffer noiseBuffer = new ComputeBuffer(
			512,
			sizeof(float)
		);

		ComputeBuffer triBuffer = new ComputeBuffer(
			2048,
			sizeof(float)*9*2,
			ComputeBufferType.Append
		);

		ComputeBuffer triCountBuffer = new ComputeBuffer (1, sizeof (int), ComputeBufferType.Raw);

		sharedProperties.densityShader.SetBuffer(0, "noiseValues", noiseBuffer);
		sharedProperties.densityShader.Dispatch(0, 1, 1, 1);
		float[] noiseValues = new float[512];

		noiseBuffer.GetData(noiseValues);

		sharedProperties.marchingCubesShader.SetBuffer(0, "noiseValues", noiseBuffer);
		sharedProperties.marchingCubesShader.SetBuffer(0, "resultTriangles", triBuffer);
		sharedProperties.marchingCubesShader.Dispatch(0, 1, 1, 1);

		ComputeBuffer.CopyCount(triBuffer, triCountBuffer, 0);

		int[] triCountArray = { 0 };
        triCountBuffer.GetData(triCountArray);
        int numTris = triCountArray[0];
		triCountBuffer.Release();
		Debug.Log(numTris);

		ResultTriangle[] resultTriangles = new ResultTriangle[numTris];
		triBuffer.GetData(resultTriangles);

		noiseBuffer.Release();
		triBuffer.Release();

		MakeMesh(resultTriangles);
	}

	public void Dispose() {
		float distanceFromPlayer = Vector3.Distance(transform.position, sharedProperties.player.position);

		if (distanceFromPlayer > sharedProperties.maxDistanceFromChunk) {
			if (mesh != null) {
				if (Application.isPlaying) {
					Destroy(mesh);
				}
				else {
					DestroyImmediate(mesh);
				}
			}

			parent.DeleteChunk(properties.chunkIndex);
		}
	}

	public void Destroy() {
		Destroy(gameObject);
	}

	private void OnEnable() {
		meshFilter = gameObject.GetComponent<MeshFilter>();
	}
}