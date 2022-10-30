using System;
using System.Collections;
using UnityEngine;

public class Chunk : MonoBehaviour, IRenderable, IDisposable
{
	public Terrain parent;
	public Mesh mesh;
	public MeshFilter meshFilter;
	public SharedChunkProperties sharedProperties;
	public ChunkProperties properties;
	private IEnumerator checkDistanceCoroutine;
	private ComputeBuffer noiseBuffer;

	private void OnEnable() {
		meshFilter = gameObject.GetComponent<MeshFilter>();
		mesh = new Mesh();
	}

	public void SetOwnMesh() {
		meshFilter.sharedMesh = mesh;
	}

	public void Render(Action doneFunction) {
		SetShaderValues();

		doneFunction();

		checkDistanceCoroutine = checkDistance();
		StartCoroutine(checkDistanceCoroutine);
	}

	public IEnumerator checkDistance() {
		while (true) {
			if (Vector3.Distance(
					sharedProperties.player.position,
					transform.position
				) > sharedProperties.maxDistanceFromChunk) {
				this.Dispose();
			}

			yield return new WaitForSecondsRealtime(0.5f);
		}
	}

	public void SetShaderValues() {
		properties.chunkType.SetShaderFields(sharedProperties.densityShader);

		sharedProperties.densityShader.SetInt("seed", sharedProperties.seed);

		sharedProperties.densityShader.SetInts(
			"chunkIndex",
			properties.chunkIndex.x,
			properties.chunkIndex.y,
			properties.chunkIndex.z
		);
	}

	public void MakeMesh() {
		noiseBuffer = new ComputeBuffer(
			(int)Math.Pow(sharedProperties.size, 3),
			sizeof(float)
		);

		sharedProperties.densityShader.SetBuffer(0, "noiseValues", noiseBuffer);

		sharedProperties.densityShader.Dispatch(0, 1, 1, 1);
	}

	public void Dispose() {
		if (mesh != null) {
			if (Application.isPlaying) {
				Destroy(meshFilter.sharedMesh);
				Destroy(mesh);
			}
			else {
				DestroyImmediate(meshFilter.sharedMesh);
				DestroyImmediate(mesh);
			}
		}
	}
}