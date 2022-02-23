using UnityEngine;

public class Chunk : MonoBehaviour {
    [SerializeField]
    private ComputeShader _marchingCubeShader;

    // [SerializeField]
    // private ComputeShader _terrainGenShader;

    [SerializeField]
    [HideInInspector]
    public ChunkOptions options;

    void OnDrawGizmos() {
        Gizmos.DrawWireCube(
            transform.position + (Vector3.one * options.distanceBetweenPoints * options.pointsPerAxis * 0.5f),
            Vector3.one * options.distanceBetweenPoints * options.pointsPerAxis
        );
    }

    public void BakeMesh() {
        gameObject.GetComponent<MeshFilter>().mesh = ChunkBaker.BakeChunkMesh(_marchingCubeShader, options);
    }
}
