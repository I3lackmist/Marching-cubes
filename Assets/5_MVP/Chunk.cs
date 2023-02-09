using System;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour, IRenderable, IDisposable
{
	private const int maxTriangles = 7*7*7 * 5;
	private const int maxPointsInCube = 8*8*8;

	public Terrain parent;
	private Mesh mesh;
	public SharedChunkProperties sharedProperties;
	public ChunkProperties properties;

	private MeshFilter meshFilter;
	private MeshRenderer meshRenderer;
	private bool visible = false;
	
	public bool Visible { 
		get => visible;
	}

	ComputeBuffer noiseBuffer;
	ComputeBuffer triBuffer;
	ComputeBuffer triCountBuffer;

	private bool isVisibleFrom(Bounds bounds, Camera camera) {
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
        return GeometryUtility.TestPlanesAABB(planes, bounds);
	}

	public void FixedUpdate() {
		bool visibleLastFrame = visible;

		visible = isVisibleFrom(
			new Bounds(transform.position, Vector3.one * sharedProperties.worldSize * 1.5f), 
			Camera.main
		);

		if (visible != visibleLastFrame) {
			meshRenderer.enabled = visible;
		}
	}

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
		bool success = DispatchShaders();

		doneFunction();

		ReleaseBuffers();
		
		if (!success) {
			parent.DeleteChunk(properties.chunkIndex);
			return;
		} 

		parent.EnqueueDispose(properties.chunkIndex);
	}

	private void SetShaderValues() {
		properties.chunkType.SetShaderFields(sharedProperties.densityShader, sharedProperties.marchingCubesShader);

		sharedProperties.densityShader.SetInt("seed", sharedProperties.seed);
		sharedProperties.densityShader.SetFloat("size", sharedProperties.size);
		
		SetChunkIndexShaderValues(sharedProperties.densityShader);

		foreach (ComputeShader filterShader in properties.chunkType.filterShaders) {
			properties.chunkType.SetShaderFields(filterShader);
			filterShader.SetInt("seed", sharedProperties.seed);
			filterShader.SetFloat("size", sharedProperties.size);
			SetChunkIndexShaderValues(filterShader);
		}

		sharedProperties.marchingCubesShader.SetFloat("distanceBetweenPoints", sharedProperties.worldSize/7);
	}

	private void SetChunkIndexShaderValues(ComputeShader shader) {
		shader.SetFloats("chunkIndex", new float[] {
			properties.chunkIndex.x,
			properties.chunkIndex.y,
			properties.chunkIndex.z
		});
	}

	private bool DispatchShaders() {
		noiseBuffer = new ComputeBuffer(
			maxPointsInCube,
			sizeof(float)
		);

		triBuffer = new ComputeBuffer(
			maxTriangles,
			sizeof(float) * 9, // Bug: HLSL float 32 bits/4 bytes, yet needs 72 bytes for some reason?
			ComputeBufferType.Append
		);

		triCountBuffer = new ComputeBuffer (1, sizeof (int), ComputeBufferType.Raw);
		triBuffer.SetCounterValue(0);

		sharedProperties.densityShader.SetBuffer(0, "noiseValues", noiseBuffer);
		sharedProperties.densityShader.Dispatch(0, 1, 1, 1);

		foreach (ComputeShader shader in properties.chunkType.filterShaders) {
			shader.SetBuffer(0, "noiseValues", noiseBuffer);
			shader.Dispatch(0, 1, 1, 1);
		}

		sharedProperties.marchingCubesShader.SetBuffer(0, "noiseValues", noiseBuffer);
		
		sharedProperties.marchingCubesShader.SetBuffer(0, "resultTriangles", triBuffer);
		sharedProperties.marchingCubesShader.Dispatch(0, 1, 1, 1);
		ComputeBuffer.CopyCount(triBuffer, triCountBuffer, 0);

		int[] triCountArray = { 0 };
        triCountBuffer.GetData(triCountArray);

        int numTris = triCountArray[0];

		if (numTris < 0 || numTris > maxTriangles) {
			Debug.LogWarning($"Number of triangles is incorrect. Value: {numTris}");
		
			return false;
		}

		ResultTriangle[] resultTriangles = new ResultTriangle[numTris];
		triBuffer.GetData(resultTriangles, 0, 0, numTris);

		MakeMesh(resultTriangles);

		return true;
	}

	private void ForceReleaseBuffers() {
		noiseBuffer.Release();
		triBuffer.Release();
		triCountBuffer.Release();
	}

	private void ReleaseBuffers() {
		if (noiseBuffer != null) {
			noiseBuffer.Release();
			noiseBuffer.Dispose();
		}

		if (triBuffer != null) {
			triBuffer.Release();
			triBuffer.Dispose();
		}

		if (triCountBuffer != null) {
			triCountBuffer.Release();
			triCountBuffer.Dispose();
		}
	}

	public void Dispose() {
		float distanceFromPlayer = Vector3.Distance(transform.position, sharedProperties.player.position);

		if (distanceFromPlayer > sharedProperties.maxDistanceFromChunk) {
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
		DisposeMesh();
		Destroy(gameObject);
	}

	private void OnEnable() {
		meshRenderer = gameObject.GetComponent<MeshRenderer>();
		meshFilter = gameObject.GetComponent<MeshFilter>();
	}
}