using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEngine;
using MarchingCubes.Common.Helpers;

namespace MarchingCubes.Chunking.Classes 
{
	public class Terrain : MonoBehaviour
	{
		[SerializeField]
		public TerrainProperties properties;

		[SerializeField]
		public QueueProperties queueProperties;

		[SerializeField]
		public SharedChunkProperties sharedChunkProperties;

		[SerializeField]
		public DisposeQueue disposeQueue;

		[SerializeField]
		public RenderQueue renderQueue;

		[SerializeField]
		public Dictionary<Vector3Int, Chunk> chunks;

		[SerializeField]
		public int chunkCount;

		private ChunkBrush chunkBrush;

		private List<Vector3Int> brushPoints;

		private void OnEnable() 
		{
			sharedChunkProperties.player = Camera.main.transform;

			chunks = new Dictionary<Vector3Int, Chunk>();
			chunkBrush = new ChunkBrush();

			chunkBrush.Size = properties.brushSize;
			brushPoints = chunkBrush.Brush;
		}

		public IEnumerator Cycle() 
		{
			while(true) {
				CycleAction();

				yield return new WaitForSeconds(queueProperties.cycleLength);
			}
		}

		public void CycleAction() 
		{
			if (chunks.Count >= properties.chunkLimit) return;

			MakeChunks(
				GetNewPointsAroundPlayer()
					.Take(queueProperties.processPerCycle)
			);
		}

		private void Start() 
		{
			if (!queueProperties.processInUpdate) StartCoroutine(Cycle());
		}

		private void Update() 
		{
			if (queueProperties.processInUpdate) CycleAction();

			sharedChunkProperties.playerGridPosition = GridHelper.ChunkIndexFromPosition(
				sharedChunkProperties.player.position, 
				sharedChunkProperties.worldSize
			);

		}

		private void FixedUpdate() {
			chunkCount = chunks.Count();
		}

		private List<Vector3Int> GetNewPointsAroundPlayer() 
		{
			return brushPoints
				.Select(point =>
					new Vector3Int(point.x, point.y, point.z) + GridHelper.ChunkIndexFromPosition(
						sharedChunkProperties.player.position, 
						sharedChunkProperties.worldSize
					)
				)
				.Except(chunks.Keys)
				.ToList();
		}

		private void MakeChunks(IEnumerable<Vector3Int> points) 
		{
			foreach (var point in points) 
			{
				Vector3 chunkPosition =
					new Vector3(point.x, point.y, point.z) * sharedChunkProperties.worldSize;

				Chunk chunk = Instantiate(
					properties.chunkPrefab,
					chunkPosition,
					Quaternion.identity,
					sharedChunkProperties.parentTransform ?? transform
				).GetComponent<Chunk>();

				chunks.Add(point, chunk);

				chunk.parent = this;
				chunk.sharedProperties = sharedChunkProperties;
				chunk.properties.chunkIndex = point;
				chunk.properties.biome = properties.biomes[0];

				renderQueue.Enqueue(chunk);
			}
		}

		public void EnqueueDispose(Vector3Int chunkIndex) 
		{
			disposeQueue.Enqueue(chunks[chunkIndex]);
		}

		public void DeleteChunk(Vector3Int chunkIndex) 
		{
			Chunk chunk = chunks[chunkIndex];
			chunks.Remove(chunkIndex);
			disposeQueue.Unqueue(chunk);
			chunk.Destroy();
		}
	}
}
