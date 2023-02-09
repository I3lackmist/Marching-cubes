using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MarchingCubeField : MonoBehaviour {
    
    [SerializeField]
    public float distanceBetweenPoints = 1f;

    [SerializeField]
    public int numPointsAlongAxis = 3;

    [SerializeField]
    public float isoLevel = 0.5f;

    [SerializeField]
    public float waitBetweenCubes = 0.025f;

    private MarchingCubeFieldData _data = new MarchingCubeFieldData();
    private Mesh _mesh;
    private MeshFilter _meshFilter;
    private List<Vector3> gizmoDrawCubes = new List<Vector3>();

    public void OnDrawGizmos() {
        if (!gizmoDrawCubes.Any()) return;

        Gizmos.color = Color.gray;
        int length = gizmoDrawCubes.Count - 1;

        for (int i = 0; i < length; i++) {
            Gizmos.DrawWireCube(gizmoDrawCubes[i] + Vector3.one * distanceBetweenPoints/2f, Vector3.one);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(gizmoDrawCubes[length] + Vector3.one * distanceBetweenPoints/2f, Vector3.one);
    }

    public void Start() {
        MarchingCubes(waitBetweenCubes);
    }

    public void GenerateValues() {
        _data.distanceBetweenPoints = distanceBetweenPoints;
        _data.numPointsAlongAxis = numPointsAlongAxis;

        _data.generateRandomValues();
        // _data.generateCubeValues();

    }

    public void MarchingCubes(float secondsWait) {
        if (_mesh != null) {
            if (Application.isPlaying) {
                Destroy(_mesh);
            }
            else {
                DestroyImmediate(_mesh);
            }
        }
        
        GenerateValues();
        _meshFilter = gameObject.GetComponent<MeshFilter>();
        _mesh = new Mesh();

        StartCoroutine(MarchingCubesCoroutine(secondsWait));
    }

    private IEnumerator MarchingCubesCoroutine(float secondsWait) {
        int midpointVertCount = 0;
        var newVerts = new List<Vector3>();
        var newTris = new List<int>();

        yield return new WaitForSeconds(1f);

        for (int y = 0; y < _data.numPointsAlongAxis-1; y++) {
            for (int z = 0; z < _data.numPointsAlongAxis-1; z++) {
                for (int x = 0; x < _data.numPointsAlongAxis-1; x++) {

                    gizmoDrawCubes.Add(new Vector3(x, y, z));

                    float[] cubeValues = {
                        _data.getPointValue(new Vector3Int(x,   y,   z)),
                        _data.getPointValue(new Vector3Int(x+1, y,   z)),
                        _data.getPointValue(new Vector3Int(x+1, y,   z+1)),
                        _data.getPointValue(new Vector3Int(x,   y,   z+1)),
                        _data.getPointValue(new Vector3Int(x,   y+1, z)),
                        _data.getPointValue(new Vector3Int(x+1, y+1, z)),
                        _data.getPointValue(new Vector3Int(x+1, y+1, z+1)),
                        _data.getPointValue(new Vector3Int(x,   y+1, z+1))
                    };

                    Vector3[] cubePositions = {
                        _data.getPointPosition(new Vector3Int(x,   y,   z)),
                        _data.getPointPosition(new Vector3Int(x+1, y,   z)),
                        _data.getPointPosition(new Vector3Int(x+1, y,   z+1)),
                        _data.getPointPosition(new Vector3Int(x,   y,   z+1)),
                        _data.getPointPosition(new Vector3Int(x,   y+1, z)),
                        _data.getPointPosition(new Vector3Int(x+1, y+1, z)),
                        _data.getPointPosition(new Vector3Int(x+1, y+1, z+1)),
                        _data.getPointPosition(new Vector3Int(x,   y+1, z+1))
                    };

                    int index = 0;
                    for (int i = 0; i < 8; i++) {
                        if (cubeValues[i] > isoLevel) {
                            index += 1 << i;
                        }
                    }

                    int[] midpointIndexes = TriangulationHelper.TriangulationTable[index];

                    if (midpointIndexes.Length > 0) {
                        Vector3[] midpointPositions = MidpointHelper.GetMidpoints(midpointIndexes, cubePositions);

                        newVerts.AddRange(midpointPositions);

                        for (int i = 0; i < midpointIndexes.Length; i++) {
                            newTris.Add(i + midpointVertCount);
                        }
                    }

                    midpointVertCount += midpointIndexes.Length;

                    _mesh.vertices = newVerts.ToArray();
                    _mesh.triangles = newTris.ToArray();

                    _mesh.RecalculateNormals();
                    _mesh.RecalculateBounds();

                    _meshFilter.mesh = _mesh;

                    yield return new WaitForSeconds(secondsWait);
                }
            }
        }

        yield return null;
    }
}
