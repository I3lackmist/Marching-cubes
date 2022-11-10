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

	ComputeBuffer noiseBuffer;
	ComputeBuffer triBuffer;
	ComputeBuffer triCountBuffer;
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
		sharedProperties.densityShader.SetFloat("size", sharedProperties.size);

		properties.chunkType.SetShaderFields(sharedProperties.densityShader, sharedProperties.marchingCubesShader);

		sharedProperties.densityShader.SetFloat("X", properties.chunkIndex.x);
		sharedProperties.densityShader.SetFloat("Y", properties.chunkIndex.y);
		sharedProperties.densityShader.SetFloat("Z", properties.chunkIndex.z);

		sharedProperties.marchingCubesShader.SetFloat("distanceBetweenPoints", sharedProperties.worldSize/7);
	}

	private void DispatchShaders() {
		noiseBuffer = new ComputeBuffer(
			512,
			sizeof(float)
		);

		triBuffer = new ComputeBuffer(
			2048,
			sizeof(float) * 9, // Bug: HLSL float 32 bits/4 bytes, yet needs 72 bytes for some reason?
			ComputeBufferType.Append
		);

		triCountBuffer = new ComputeBuffer (1, sizeof (int), ComputeBufferType.Raw);

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

		ResultTriangle[] resultTriangles = new ResultTriangle[numTris];
		triBuffer.GetData(resultTriangles);

		MakeMesh(resultTriangles);
	}

	private void ReleaseBuffers() {
		if (noiseBuffer != null) {
			noiseBuffer.Release();
			triBuffer.Release();
			triCountBuffer.Release();
		}
	}

	public void Dispose() {
		float distanceFromPlayer = Vector3.Distance(transform.position, sharedProperties.player.position);

		if (distanceFromPlayer > sharedProperties.maxDistanceFromChunk) {
			DisposeMesh();
			ReleaseBuffers();

			parent.DeleteChunk(properties.chunkIndex);
		}
	}

	private void DisposeMesh() {
		if (mesh != null) {
			if (Application.isPlaying) {
				Destroy(mesh);
			}
			else {
				DestroyImmediate(mesh);
			}
		}
	}

	private void OnDestroy() {
		DisposeMesh();
		ReleaseBuffers();
	}

	public void Destroy() {
		Destroy(gameObject);
	}

	private void OnEnable() {
		meshFilter = gameObject.GetComponent<MeshFilter>();
	}
}