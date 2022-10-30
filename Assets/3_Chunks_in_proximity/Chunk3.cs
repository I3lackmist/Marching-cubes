using UnityEngine;

public class Chunk3 : MonoBehaviour
{
	public Vector2Int chunkIndex;
	public float maxDistance;
	public Terrain3 parent;

    void Update()
    {
        if (Vector3.Distance(transform.position, parent.player.position) > maxDistance) {
			parent.chunks.Remove(chunkIndex);
			parent.renderQueue.Remove(gameObject);
			Destroy(gameObject);
		}
    }
}
