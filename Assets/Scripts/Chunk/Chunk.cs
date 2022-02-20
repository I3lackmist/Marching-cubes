using UnityEngine;

public class Chunk : MonoBehaviour {
    [SerializeField]
    private ComputeShader _shader;

    [SerializeField]
    [HideInInspector]
    public ChunkOptions options;

    void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position + (Vector3.one * options.distanceBetweenPoints*4),  Vector3.one * options.distanceBetweenPoints*8);
    }

    public void BakeMesh() {
        gameObject.GetComponent<MeshFilter>().mesh = ChunkBaker.BakeChunkMesh(_shader, options);
    }
}
