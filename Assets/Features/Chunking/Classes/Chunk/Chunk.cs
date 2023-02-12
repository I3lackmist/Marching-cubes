using System;
using UnityEngine;
using MarchingCubes.Common.Interfaces;
using MarchingCubes.Common.Structs;
using MarchingCubes.Common.Helpers;
using System.Collections;

namespace MarchingCubes.Chunking.Classes 
{
	public class Chunk : MonoBehaviour, IDisposable, IRenderable 
	{
		public Terrain parent;
		private Mesh mesh;
		public SharedChunkProperties sharedProperties;
		public ChunkProperties properties;

		private MeshFilter meshFilter;
		private MeshRenderer meshRenderer;
		private static int maxTriangles = 7*7*7 * 5;
		private static int maxPointsInCube = 8*8*8;
		private static float[] noiseBufferInit = new float[maxPointsInCube];
		private ComputeBuffer noiseBuffer;
		private ComputeBuffer triBuffer;
		private ComputeBuffer triCountBuffer;
		private bool visible = false;
		
		public bool Visible 
		{ 
			get => visible;
		}

		public void Render(Action Done) 
		{
			SetDensityShaderPosition();
			InitializeBuffers();
			bool success = DispatchShaders();

			Done();
			ReleaseBuffers();
			
			if (!success) 
			{
				parent.DeleteChunk(properties.chunkIndex);
				return;
			}
		}

		private void MakeMesh(ResultTriangle[] resultTris) 
		{
			mesh = MeshBaker.BakeMesh(resultTris);
			meshFilter.sharedMesh = mesh;
		}

		private void SetDensityShaderPosition() 
		{
			sharedProperties.densityShader.SetFloats("chunkIndex", new float[] {
				properties.chunkIndex.x,
				properties.chunkIndex.y,
				properties.chunkIndex.z
			});
			
			sharedProperties.overworldCutoffShader.SetFloats("chunkIndex", new float[] {
				properties.chunkIndex.x,
				properties.chunkIndex.y,
				properties.chunkIndex.z
			});
		}

		private void InitializeBuffers() 
		{
			noiseBuffer = new ComputeBuffer(
				maxPointsInCube,
				sizeof(float)
			);

			noiseBuffer.SetData(noiseBufferInit);

			sharedProperties.densityShader.SetBuffer(0, "noiseValues", noiseBuffer);

			triBuffer = new ComputeBuffer(
				maxTriangles,
				sizeof(float) * 9,
				ComputeBufferType.Append
			);

			triCountBuffer = new ComputeBuffer (1, sizeof (int), ComputeBufferType.Raw);
			
			triBuffer.SetCounterValue(0);
		}

		private bool DispatchShaders() 
		{
			foreach (var shaderPass in properties.biome.densityShaderPasses) 
			{
				shaderPass.SetProperties(sharedProperties.densityShader);
				sharedProperties.densityShader.Dispatch(0, 1, 1, 1);
			}

			sharedProperties.overworldCutoffShader.SetBuffer(0, "noiseValues", noiseBuffer);

			foreach (var shaderPass in properties.biome.overworldShaderPasses) 
			{
				shaderPass.SetProperties(sharedProperties.overworldCutoffShader);
				sharedProperties.overworldCutoffShader.Dispatch(0, 1, 1, 1);
			}

			sharedProperties.marchingCubesShader.SetBuffer(0, "noiseValues", noiseBuffer);
			sharedProperties.marchingCubesShader.SetBuffer(0, "resultTriangles", triBuffer);

			foreach (var shaderPass in properties.biome.marchingCubeShaderPasses) 
			{
				shaderPass.SetProperties(sharedProperties.marchingCubesShader);
				sharedProperties.marchingCubesShader.Dispatch(0, 1, 1, 1);
			}
			
			ComputeBuffer.CopyCount(triBuffer, triCountBuffer, 0);

			int[] triCount = { 0 };
			triCountBuffer.GetData(triCount);
			int numTris = triCount[0];

			if (numTris < 0 || numTris > maxTriangles) 
			{
				Debug.LogWarning($"Number of triangles is incorrect. Value: {numTris}");
				return false;
			}

			ResultTriangle[] resultTriangles = new ResultTriangle[numTris];
			triBuffer.GetData(resultTriangles, 0, 0, numTris);

			MakeMesh(resultTriangles);

			return true;
		}

		public void Dispose() 
		{
			parent.DeleteChunk(properties.chunkIndex);
		}

		private void ReleaseBuffers()
		{
			if (noiseBuffer != null) 
			{
				noiseBuffer.Release();
				noiseBuffer.Dispose();
			}

			if (triBuffer != null) 
			{
				triBuffer.Release();
				triBuffer.Dispose();
			}

			if (triCountBuffer != null) 
			{
				triCountBuffer.Release();
				triCountBuffer.Dispose();
			}
		}

		private void DisposeMesh() 
		{
			if (mesh != null) {
				if (Application.isPlaying) 
				{
					Destroy(mesh);
				}
				else 
				{
					DestroyImmediate(mesh);
				}
			}
		}

		private bool tbd = false;
		private void FixedUpdate() 
		{
			VisibilityCheck();

			if (!tbd) tbd = DistanceCheck();
		}
		

		private void OnDestroy() 
		{
			DisposeMesh();
			ReleaseBuffers();
		}

		private void OnEnable() 
		{
			meshRenderer = gameObject.GetComponent<MeshRenderer>();
			meshFilter = gameObject.GetComponent<MeshFilter>();
		}

		private void VisibilityCheck() 
		{
			bool visibleLastFrame = visible;

			visible = GeometryUtility.TestPlanesAABB(
				GeometryUtility.CalculateFrustumPlanes(Camera.main), 
				new Bounds(transform.position, Vector3.one * sharedProperties.worldSize * 1.5f) 
			);

			if (visible != visibleLastFrame)
			{
				meshRenderer.enabled = visible || sharedProperties.alwaysVisible;
			}
		}
		
		private bool DistanceCheck() 
		{

			int distanceFromPlayer = Mathf.FloorToInt(
				Vector3Int.Distance(
					GridHelper.ChunkIndexFromPosition(
						transform.position, 
						sharedProperties.worldSize
					), 
					sharedProperties.playerGridPosition
				)
			);

			if (distanceFromPlayer > sharedProperties.maxDistanceFromChunk) 
			{	
				parent.EnqueueDispose(properties.chunkIndex);

				return true;
			}

			return false;
		}

		public void Destroy() 
		{
			DisposeMesh();
			Destroy(gameObject);
		}
	}
}