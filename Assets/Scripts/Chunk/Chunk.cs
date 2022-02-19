using UnityEngine;
public class Chunk : MonoBehaviour {
    [SerializeField]
    private ComputeShader _shader;

    [SerializeField]
    [HideInInspector]
    public float distanceBetweenPoints;


    public void OnDrawGizmos() {
        Vector3 origin = transform.position;

        for(int x = 0; x < 16; x++) {
            for(int y = 0; y < 16; y++) {
                for(int z = 0; z < 16; z++) {
                    Gizmos.DrawCube(origin + new Vector3(x,y,z) * distanceBetweenPoints, Vector3.one * 0.25f);
                }
            }
        }
    }

    public void BakeMesh() {
        gameObject.GetComponent<MeshFilter>().mesh = ChunkBaker.BakeChunkMesh(_shader, gameObject.GetComponent<Transform>().position);
    }
}
