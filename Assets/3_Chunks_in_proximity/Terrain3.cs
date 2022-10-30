using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Terrain3 : MonoBehaviour
{
	[SerializeField]
	public Terrain3Properties terrainProperties;

	public Transform player;

	public Dictionary<Vector2Int, GameObject> chunks;
	private List<Vector2Int> chunkpoints;

	private Vector2 halfChunkSize;

	private TerrainBrush brush;

	public List<GameObject> renderQueue;

	private void Start() {
		player = Camera.main.transform;
		chunkpoints = new List<Vector2Int>();
		chunks = new Dictionary<Vector2Int, GameObject>();
		halfChunkSize = terrainProperties.chunkSize/2;
		brush = new TerrainBrush();
		brush.Size = terrainProperties.minDistance;
		renderQueue = new List<GameObject>();
	}

	private void Update() {
		if (renderQueue.Any()) {
			GameObject chunk = renderQueue.First();
			renderQueue.RemoveAt(0);

			chunk.GetComponent<NoiseMap>().Draw();
		}

		List<Vector2Int> brushpoints = brush.Brush
			.Select(point =>
				new Vector2Int(point.x, point.y) + ChunkIndexFromPosition(player.position)
			)
			.Except(chunks.Keys)
			.ToList();

		foreach (Vector2Int point in brushpoints) {
			if (!chunks.Keys.Contains(point)) {
				var chunkIndex = new Vector2Int(point.x, point.y);

				var newChunk = Instantiate(
					terrainProperties.chunkPrefab,
					new Vector3(point.x, 0, point.y),
					Quaternion.identity
				);

				newChunk.GetComponent<Chunk3>().parent = this;
				newChunk.GetComponent<Chunk3>().chunkIndex = chunkIndex;
				newChunk.GetComponent<Chunk3>().maxDistance = terrainProperties.maxDistance;

				renderQueue.Add(newChunk);

				chunks.Add(
					chunkIndex,
					newChunk
				);
			}
		}
	}

	public Vector2Int ChunkIndexFromPosition(Vector3 position) {
		return new Vector2Int(
			Mathf.FloorToInt(position.x/terrainProperties.chunkSize.x),
			Mathf.FloorToInt(position.z/terrainProperties.chunkSize.y)
		);
	}
}
