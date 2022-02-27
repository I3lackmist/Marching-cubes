using System;
using UnityEngine;

public class Chunk : MonoBehaviour {
    [SerializeField]
    private ComputeShader _marchingCubeShader;

    [SerializeField]
    [HideInInspector]
    public ChunkOptions options;

    void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(
            transform.position,
            Vector3.one * options.distanceBetweenPoints * options.pointsPerAxis
        );

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(options.chunkOrigin, 0.1f);
    }

    public void SetMesh(Mesh mesh) {
        gameObject.GetComponent<MeshFilter>().mesh = mesh;
    }
    public void BakeMesh() {
        Action<Mesh> setMeshAction = SetMesh;

        ChunkBaker.BakeChunkMesh(_marchingCubeShader, options, setMeshAction);
    }
}
