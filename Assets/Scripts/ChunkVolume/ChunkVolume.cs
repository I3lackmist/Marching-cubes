using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkVolume : MonoBehaviour {

    [SerializeField]
    [HideInInspector]
    private GameObject[] chunks;

    [SerializeField]
    public GameObject chunkPrefab;

    [SerializeField]
    public Vector3 volumeSize = new Vector3(1,1,1);

    [SerializeField]
    public float distanceBetweenPoints = 0.25f;

    public void CreateTerrain() {
        Vector3 center = transform.position;

        List<GameObject> newChunks = new List<GameObject>();

        for(int x = 0; x < volumeSize.x; x++) {
            for(int y = 0; y < volumeSize.y; y++) {
                for(int z = 0; z < volumeSize.z; z++) {
                    GameObject newChunk = GameObject.Instantiate(chunkPrefab, center + new Vector3(x,y,z) * 16, Quaternion.identity, transform);
                    newChunk.GetComponent<Chunk>().distanceBetweenPoints = distanceBetweenPoints;
                    newChunks.Add(newChunk);
                }
            }
        }

        chunks = newChunks.ToArray();

        foreach(GameObject chunkTransform in chunks) {
            //chunk.BakeMesh();
        }
    }
}
