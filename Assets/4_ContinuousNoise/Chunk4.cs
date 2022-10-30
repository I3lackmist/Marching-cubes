using UnityEngine;

public class Chunk4 : MonoBehaviour
{
	public Vector2Int chunkIndex;
	public float maxDistance;
	public Terrain4 parent;

    void Update()
    {
        if (Vector3.Distance(transform.position, parent.player.position) > maxDistance) {
			parent.DeleteChunk(chunkIndex);
		}
    }
}
