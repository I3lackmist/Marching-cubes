using System;
using System.Collections;
using UnityEngine;

public class Chunk : MonoBehaviour, IRenderable, IDisposable
{
	public Terrain parent;
	public Mesh mesh;
	public SharedChunkProperties sharedProperties;
	public ChunkProperties properties;

	private MeshFilter meshFilter;

	public void SetOwnMesh() {
		meshFilter.sharedMesh = mesh;
	}

	public void Render(Action doneFunction) {
		SetShaderValues();
		DispatchShaders();
		SetOwnMesh();

		doneFunction();

		parent.EnqueueDispose(properties.chunkIndex);
	}

	private void SetShaderValues() {
		sharedProperties.densityShader.SetInt("seed", sharedProperties.seed);

		properties.chunkType.SetShaderFields(sharedProperties.densityShader, sharedProperties.marchingCubesShader);

		sharedProperties.densityShader.SetInts(
			"chunkIndex",
			properties.chunkIndex.x,
			properties.chunkIndex.y,
			properties.chunkIndex.z
		);

		sharedProperties.marchingCubesShader.SetInts(
			"chunkIndex",
			properties.chunkIndex.x,
			properties.chunkIndex.y,
			properties.chunkIndex.z
		);

		sharedProperties.marchingCubesShader.SetFloat("distanceBetweenPoints", transform.localScale.x/8);

		sharedProperties.marchingCubesShader.SetFloats(
			"chunkOrigin",
			transform.position.x,
			transform.position.y,
			transform.position.z
		);
	}

	private void DispatchShaders() {
		int elemNum = (int)Math.Pow(sharedProperties.size, 3);
		ComputeBuffer noiseBuffer = new ComputeBuffer(
			elemNum,
			sizeof(float)
		);

		ComputeBuffer triBuffer = new ComputeBuffer(
			elemNum,
			sizeof(float) * 9
		);

		sharedProperties.densityShader.SetBuffer(0, "noiseValues", noiseBuffer);

		sharedProperties.densityShader.Dispatch(0, 1, 1, 1);

		sharedProperties.marchingCubesShader.SetBuffer(0, "noiseValues", noiseBuffer);

		sharedProperties.marchingCubesShader.Dispatch(0, 1, 1, 1);


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
		mesh = new Mesh();
	}
}