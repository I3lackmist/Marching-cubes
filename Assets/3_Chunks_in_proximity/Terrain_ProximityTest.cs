using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Terrain_ProximityTest : MonoBehaviour
{
	[SerializeField]
	public TerrainProperties_ProximityTest terrainProperties;

	private Transform player;

	public Dictionary<Vector2Int, GameObject> chunks;
	private List<Vector2Int> chunkpoints;

	private Vector2 halfChunkSize;

	private TerrainBrush_ProximityTest brush;

	private void Start() {
		player = Camera.main.transform;
		chunkpoints = new List<Vector2Int>();
		chunks = new Dictionary<Vector2Int, GameObject>();
		halfChunkSize = terrainProperties.chunkSize * 0.5f;
		brush = new TerrainBrush_ProximityTest();
		brush.Size = terrainProperties.minDistance;
	}

	private void Update() {
		List<Vector2Int> brushpoints = brush._brush
			.Select(point =>
				point + ChunkIndexFromPosition(player.position)
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

				newChunk.AddComponent(typeof(Chunk_ProximityTest));

				newChunk.GetComponent<Chunk_ProximityTest>().playerTransform = player;
				newChunk.GetComponent<Chunk_ProximityTest>().chunkIndex = chunkIndex;
				newChunk.GetComponent<Chunk_ProximityTest>().parent = this;
				newChunk.GetComponent<Chunk_ProximityTest>().maxDistance = terrainProperties.maxDistance;

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
