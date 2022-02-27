using System.Collections.Generic;
using UnityEngine;

public class ChunkVolume : MonoBehaviour {

    [SerializeField]
    public GameObject chunkPrefab;

    [SerializeField]
    public Vector3Int volumeSize = new Vector3Int(1, 1, 1);

    [SerializeField]
    public float distanceBetweenPoints = 0.25f;

    [SerializeField]
    public float terrainRatio = 0.5f;

    [SerializeField]
    public int pointsPerAxis = 4;

    [SerializeField]
    [HideInInspector]
    private GameObject[] chunkGameObjects;

    public void ResetChunks() {
        if(chunkGameObjects == null) return;

        foreach (GameObject chunk in chunkGameObjects) {
            DestroyImmediate(chunk);
        }
    }

    public void ResetMeshes() {
        foreach (GameObject chunk in chunkGameObjects) {
            chunk.GetComponent<MeshFilter>().mesh = new Mesh();
        }
    }

    public void MakeChunks() {
        ResetChunks();

        Vector3 volumeOrigin = transform.position;
        Vector3 halfCubeOffset = Vector3.one * (pointsPerAxis/2) * distanceBetweenPoints;
        Vector3 cubeOffset = halfCubeOffset * 2;

        List<GameObject> chunkGameObjectList = new List<GameObject>();

        for(int x = 0; x < volumeSize.x; x++) {
            for(int y = 0; y < volumeSize.y; y++) {
                for(int z = 0; z < volumeSize.z; z++) {
                    Vector3 iteratedCubeOffset = new Vector3(x,y,z);
                    iteratedCubeOffset.Scale(cubeOffset);

                    GameObject newChunk = GameObject.Instantiate(chunkPrefab, volumeOrigin + iteratedCubeOffset, Quaternion.identity);

                    newChunk.GetComponent<Chunk>().options = new ChunkOptions() {
                        distanceBetweenPoints = distanceBetweenPoints,
                        chunkOrigin = newChunk.transform.position - halfCubeOffset,
                        chunkIndex = new Vector3Int(x,y,z),
                        terrainRatio = terrainRatio,
                        pointsPerAxis = pointsPerAxis
                    };

                    chunkGameObjectList.Add(newChunk);
                }
            }
        }

        chunkGameObjects = chunkGameObjectList.ToArray();
    }

    public void MakeMeshes() {
        ResetMeshes();

        foreach(GameObject chunkGameObject in chunkGameObjects) {
            chunkGameObject.GetComponent<Chunk>().BakeMesh();
        }
    }
}
