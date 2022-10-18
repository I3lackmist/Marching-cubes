using System;
using UnityEngine;

public class Chunk: MonoBehaviour {

    [SerializeField]
    [HideInInspector]
    public ChunkProperties chunkProperties;

    [SerializeField]
	[HideInInspector]
	public VolumeProperties volumeProperties;

	[SerializeField]
	public float[] isoLevels;

	public void DestroyOrDisable() {
        if (Application.isPlaying) {
            ClearMesh();
            gameObject.SetActive(false);
        } else {
            DestroyImmediate(gameObject, false);
        }
    }

    void OnDrawGizmos() {
		if (!volumeProperties.drawGizmos) return;

		Gizmos.color = Color.white;

		Gizmos.DrawWireCube(
			transform.position,
			Vector3.one * volumeProperties.distanceBetweenPoints * 8
		);

		Gizmos.color = Color.green;
		Gizmos.DrawSphere(chunkProperties.origin, 0.1f);
    }

    public void SetMesh(Mesh mesh) {
        gameObject.GetComponent<MeshFilter>().sharedMesh = mesh;
    }

	public void ClearMesh() {
		if (gameObject.GetComponent<MeshFilter>().sharedMesh != null) {
			gameObject.GetComponent<MeshFilter>().sharedMesh.Clear();

	        if (Application.isPlaying) {
				Destroy(gameObject.GetComponent<MeshFilter>().sharedMesh);
			}
			else {
				DestroyImmediate(gameObject.GetComponent<MeshFilter>().sharedMesh);
			}
		}
	}

    public void BakeMesh() {
		ClearMesh();

        Action<Mesh> setMeshAction = SetMesh;
        ChunkBaker.BakeChunkMesh(this, setMeshAction);
    }
}
