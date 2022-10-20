using UnityEngine;

public class Chunk_ProximityTest : MonoBehaviour
{
	public Vector2Int chunkIndex;
	public Transform playerTransform;
	public float maxDistance;

	public Terrain_ProximityTest parent;

    void Update()
    {
        if (Vector3.Distance(transform.position, playerTransform.position) > maxDistance) {
			parent.chunks.Remove(chunkIndex);
			Destroy(gameObject);
		}
    }
}
