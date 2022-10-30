using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Terrain : MonoBehaviour
{
	[SerializeField]
	public TerrainProperties properties;

	[SerializeField]
	public SharedChunkProperties sharedChunkProperties;

	[SerializeField]
	public RenderQueue renderQueue;

	[SerializeField]
	public Dictionary<Vector3Int, Chunk> chunks;

	private TerrainBrush brush;

	private void OnEnable() {
		sharedChunkProperties.player = Camera.main.transform;

		chunks = new Dictionary<Vector3Int, Chunk>();
	}

	private void Start() {
		if (brush == null) {
			brush = new TerrainBrush();

			brush.Size = 3;
			brush.Is3D = true;
		}
	}

	private void Update() {
		var points = GetNewPointsAroundPlayer();

		foreach (var point in points) {
			Chunk chunk = Instantiate(
				properties.chunkPrefab,
				point,
				Quaternion.identity
			).GetComponent<Chunk>();

			chunks.Add(point, chunk);

			chunk.properties.chunkIndex = point;
			chunk.properties.chunkType = properties.chunkTypes[0];

			renderQueue.Enqueue(chunk);
		}
	}

	public Vector3Int ChunkIndexFromPosition(Vector3 position) {
		return new Vector3Int(
			Mathf.FloorToInt(position.x/sharedChunkProperties.worldSize),
			Mathf.FloorToInt(position.y/sharedChunkProperties.worldSize),
			Mathf.FloorToInt(position.z/sharedChunkProperties.worldSize)
		);
	}

	private List<Vector3Int> GetNewPointsAroundPlayer() {
		return brush.Brush
			.Select(point =>
				new Vector3Int(point.x, point.y, point.z) + ChunkIndexFromPosition(Vector3.zero)
			)
			.Except(chunks.Keys)
			.ToList();
	}
}
