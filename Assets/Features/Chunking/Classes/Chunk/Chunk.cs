using System;
using UnityEngine;
using MarchingCubes.Common.Interfaces;
using MarchingCubes.Common.Structs;
using MarchingCubes.Common.Helpers;
using MarchingCubes.ShaderPasses.Interfaces;
using MarchingCubes.ShaderPasses.Enums;

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

		private static int size = 8;
		private static int pointsInPlane = size*size;
		private static int pointsInCube = size*size*size;
		private static float[] noiseBufferInit = new float[pointsInCube];
		private ComputeBuffer noiseBuffer;
		private ComputeBuffer heightBuffer;
		private ComputeBuffer triBuffer;
		private ComputeBuffer triCountBuffer;
		private bool visible = false;
		private bool toBeDeleted = false;
		
		public bool Visible 
		{ 
			get => visible;
		}

		public void Render(Action Done) 
		{
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

		private void InitializeBuffers() 
		{
			noiseBuffer = new ComputeBuffer(
				pointsInCube,
				sizeof(float)
			);

			noiseBuffer.SetData(noiseBufferInit);

			heightBuffer = new ComputeBuffer(
				pointsInPlane,
				sizeof(float)
			);

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
			foreach (var shaderPass in properties.biome.ShaderPasses()) 
			{
				shaderPass.SetPosition(properties.chunkIndex);

				shaderPass.SetBuffer(BufferName.NoiseValues, noiseBuffer);
				shaderPass.SetBuffer(BufferName.HeightValues, heightBuffer);
				shaderPass.SetBuffer(BufferName.ResultTriangles, triBuffer);
				
				shaderPass.Execute();
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

			if (heightBuffer != null) 
			{
				heightBuffer.Release();
				heightBuffer.Dispose();
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

		private void FixedUpdate() 
		{
			VisibilityCheck();

			if (!toBeDeleted) toBeDeleted = DistanceCheck();
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