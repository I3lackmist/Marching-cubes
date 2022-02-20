using System.Collections.Generic;
using UnityEngine;

public class ChunkVolume : MonoBehaviour {

    [SerializeField]
    [HideInInspector]
    private GameObject[] chunkGameObjects;

    [SerializeField]
    public GameObject chunkPrefab;

    [SerializeField]
    public Vector3 volumeSize = new Vector3(1, 1, 1);

    [SerializeField]
    public float distanceBetweenPoints = 0.25f;

    [SerializeField]
    public float terrainRatio = 0.5f;

    public void Reset() {
        foreach (GameObject chunk in chunkGameObjects) {
            DestroyImmediate(chunk);
        }

        chunkGameObjects = new GameObject[] {};
    }

    public void CreateTerrain() {
        Vector3 center = transform.position;

        List<GameObject> newChunkGameObjects = new List<GameObject>();

        for(int x = 0; x < volumeSize.x; x++) {
            for(int y = 0; y < volumeSize.y; y++) {
                for(int z = 0; z < volumeSize.z; z++) {
                    GameObject newChunk = GameObject.Instantiate(chunkPrefab, center + new Vector3(x,y,z) * distanceBetweenPoints * 8, Quaternion.identity, transform);
                    newChunk.GetComponent<Chunk>().options = new ChunkOptions() {
                        distanceBetweenPoints = distanceBetweenPoints,
                        chunkCenter = newChunk.transform.position,
                        terrainRatio = terrainRatio
                    };

                    newChunkGameObjects.Add(newChunk);
                }
            }
        }

        chunkGameObjects = newChunkGameObjects.ToArray();

        foreach(GameObject chunkGameObject in chunkGameObjects) {
            chunkGameObject.GetComponent<Chunk>().BakeMesh();
        }
    }
}
