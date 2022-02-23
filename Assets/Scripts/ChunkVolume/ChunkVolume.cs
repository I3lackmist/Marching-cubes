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

    public void Reset() {
        foreach (GameObject chunk in chunkGameObjects) {
            DestroyImmediate(chunk);
        }

        chunkGameObjects = new GameObject[] {};
    }

    public void CreateTerrain() {
        Reset();

        Vector3 center = transform.position;

        List<GameObject> chunkGameObjectList = new List<GameObject>();

        for(int x = 0; x < volumeSize.x; x++) {
            for(int y = 0; y < volumeSize.y; y++) {
                for(int z = 0; z < volumeSize.z; z++) {
                    GameObject newChunk = GameObject.Instantiate(chunkPrefab, center + new Vector3(x,y,z) * distanceBetweenPoints * pointsPerAxis, Quaternion.identity, transform);

                    newChunk.GetComponent<Chunk>().options = new ChunkOptions() {
                        distanceBetweenPoints = distanceBetweenPoints,
                        chunkOrigin = newChunk.transform.position - Vector3.one * pointsPerAxis/2 * distanceBetweenPoints,
                        terrainRatio = terrainRatio,
                        pointsPerAxis = pointsPerAxis
                    };

                    chunkGameObjectList.Add(newChunk);
                }
            }
        }

        chunkGameObjects = chunkGameObjectList.ToArray();

        foreach(GameObject chunkGameObject in chunkGameObjects) {
            chunkGameObject.GetComponent<Chunk>().BakeMesh();
        }
    }
}
