using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkVolume: MonoBehaviour {

    [SerializeField]
    public GameObject chunkPrefab;

    [SerializeField]
	public VolumeProperties volumeProperties;

    private GameObject[] chunkGameObjects = Array.Empty<GameObject>();

    public void ResetChunks() {
		ResetMeshes();

        foreach (GameObject chunk in chunkGameObjects) {
            chunk.GetComponent<Chunk>().ClearMesh();
			Destroy(chunk);
        }

		chunkGameObjects = Array.Empty<GameObject>();
    }


    public void ResetMeshes() {
        foreach (GameObject chunk in chunkGameObjects) {
            chunk.GetComponent<Chunk>().ClearMesh();
            chunk.GetComponent<MeshFilter>().sharedMesh = new Mesh();
        }
    }

    public void MakeChunks() {
		ResetChunks();

        Vector3 volumeOrigin = transform.position;
        Vector3 halfCubeOffset = Vector3.one * 4 * volumeProperties.distanceBetweenPoints;
        Vector3 cubeOffset = halfCubeOffset * 2;

        List<GameObject> chunkGameObjectList = new List<GameObject>();

        for(int x = 0; x < volumeProperties.volumeSize.x; x++) {
            for(int y = 0; y < volumeProperties.volumeSize.y; y++) {
                for(int z = 0; z < volumeProperties.volumeSize.z; z++) {
					Vector3 iteratedCubeOffset = new Vector3(x,y,z);
                    iteratedCubeOffset.Scale(cubeOffset);

                    GameObject newChunk = GameObject.Instantiate(chunkPrefab, volumeOrigin + iteratedCubeOffset, Quaternion.identity);

					newChunk.GetComponent<Chunk>().volumeProperties = volumeProperties;

					newChunk.GetComponent<Chunk>().chunkProperties = new ChunkProperties() {
                        origin = newChunk.transform.position - halfCubeOffset,
                        index = new Vector3Int(x, y, z),
                    };

                    chunkGameObjectList.Add(newChunk);
                }
            }
        }

        chunkGameObjects = chunkGameObjectList.ToArray();
    }

	private IEnumerator MakeMeshesCoroutine() {
		foreach(GameObject chunkGameObject in chunkGameObjects) {
            chunkGameObject.GetComponent<Chunk>().BakeMesh();
			yield return new WaitForEndOfFrame();
		}
	}

    public void MakeMeshes() {
        ResetMeshes();

		StartCoroutine(MakeMeshesCoroutine());
    }
}
