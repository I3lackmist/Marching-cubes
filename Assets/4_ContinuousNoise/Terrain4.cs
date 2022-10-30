using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Terrain4 : MonoBehaviour
{
	[SerializeField]
	public Terrain4Properties terrainProperties;

	[SerializeField]
	public Noise4Properties noiseProperties;

	public Transform player;

	public Dictionary<Vector2Int, GameObject> chunks;

	private TerrainBrush brush;

	public RenderQueue4 renderQueue;

	private void Start() {
		player = Camera.main.transform;
		chunks = new Dictionary<Vector2Int, GameObject>();
		renderQueue = new RenderQueue4(noiseProperties);

		brush = new TerrainBrush();
		brush.Size = terrainProperties.minDistance;
	}

	private void Update() {
		renderQueue.Update();

		List<Vector2Int> brushpoints = brush.Brush
			.Select(point =>
				new Vector2Int(point.x, point.z) + ChunkIndexFromPosition(player.position)
			)
			.Except(chunks.Keys)
			.ToList();

		foreach (Vector2Int point in brushpoints) {
			if (!chunks.Keys.Contains(point)) {
				Vector2Int chunkIndex = new Vector2Int(point.x, point.y);
				GameObject newChunkObject = Instantiate(
					terrainProperties.chunkPrefab,
					new Vector3(point.x, 0, point.y),
					Quaternion.Euler(90,0,0)
				);

				Chunk4 newChunk = newChunkObject.GetComponent<Chunk4>();
				NoiseMap4 newChunkNoiseMap = newChunkObject.GetComponent<NoiseMap4>();

				newChunk.parent = this;
				newChunk.chunkIndex = chunkIndex;
				newChunk.maxDistance = terrainProperties.maxDistance;

				newChunkNoiseMap.chunkIndex = chunkIndex;

				newChunkNoiseMap.SetOwnTexture(
					noiseProperties.width,
					noiseProperties.height
				);

				renderQueue.Enqueue(newChunkNoiseMap);

				chunks.Add(
					chunkIndex,
					newChunkObject
				);
			}
		}
	}

	public void DeleteChunk(Vector2Int chunkIndex) {
		renderQueue.Unqueue(chunks[chunkIndex].GetComponent<NoiseMap4>());
		Destroy(chunks[chunkIndex]);
		chunks.Remove(chunkIndex);
	}

	public Vector2Int ChunkIndexFromPosition(Vector3 position) {
		return new Vector2Int(
			Mathf.FloorToInt(position.x/terrainProperties.chunkSize.x),
			Mathf.FloorToInt(position.z/terrainProperties.chunkSize.y)
		);
	}
}
