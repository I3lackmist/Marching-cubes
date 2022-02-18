using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchingCubeField : MonoBehaviour {
    private MarchingCubeFieldData _data = new MarchingCubeFieldData();
    private Mesh _mesh;
    private MeshFilter _meshFilter;
    [SerializeField]
    public float distanceBetweenPoints = 1f;
    [SerializeField]
    public Vector3 fieldCenter = Vector3.zero;
    [SerializeField]
    public int numPointsAlongAxis = 3;

    private int _x = 0;
    private int _y = 0;
    private int _z = 0;

    public int TerrainThreshold {
        get {
            return _data.terrainThreshold;
        }
        set {
            _data.terrainThreshold = value;
        }
    }
    public void Start() {
        _meshFilter = gameObject.GetComponent<MeshFilter>();
        _mesh = new Mesh();

        GenerateValues();

        StartCoroutine(MarchingCubesCoroutine());
    }

    public void GenerateValues() {
        _data.distanceBetweenPoints = distanceBetweenPoints;
        _data.fieldCenter = fieldCenter;
        _data.numPointsAlongAxis = numPointsAlongAxis;

        _data.generateValues();

        _meshFilter = gameObject.GetComponent<MeshFilter>();
        _meshFilter.mesh = null;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.white;
        for (int x = 0; x < _data.numPointsAlongAxis; x++) {
            for (int y = 0; y < _data.numPointsAlongAxis; y++) {
                for (int z = 0; z < _data.numPointsAlongAxis; z++) {
                    if (_data.getPointValue(x,y,z)) Gizmos.DrawSphere(_data.getPointPosition(x,y,z), 0.1f);
                }
            }
        }

        if (_x < _data.numPointsAlongAxis - 1 && _y < _data.numPointsAlongAxis - 1 && _z < _data.numPointsAlongAxis - 1) {
            Gizmos.DrawWireCube(
                _data.getPointPosition(_x, _y, _z) + Vector3.one * 0.5f * _data.distanceBetweenPoints,
                Vector3.one * _data.distanceBetweenPoints
            );
        }
    }

    private List<Vector3> newVerts = new List<Vector3>();
    private List<int> newTris = new List<int>();

    public void MarchingCubes() {
        _meshFilter = gameObject.GetComponent<MeshFilter>();
        _mesh = new Mesh();

        int midpointVertCount = 0;

        for (int y = -1; y < _data.numPointsAlongAxis+1; y++) {
            for (int z = -1; z < _data.numPointsAlongAxis+1; z++) {
                for (int x = -1; x < _data.numPointsAlongAxis+1; x++) {
                    bool[] cubeValues = {
                        _data.getPointValue(x,   y,   z),
                        _data.getPointValue(x+1, y,   z),
                        _data.getPointValue(x+1, y,   z+1),
                        _data.getPointValue(x,   y,   z+1),
                        _data.getPointValue(x,   y+1, z),
                        _data.getPointValue(x+1, y+1, z),
                        _data.getPointValue(x+1, y+1, z+1),
                        _data.getPointValue(x,   y+1, z+1)
                    };

                    Vector3[] cubePositions = {
                        _data.getPointPosition(x,   y,   z),
                        _data.getPointPosition(x+1, y,   z),
                        _data.getPointPosition(x+1, y,   z+1),
                        _data.getPointPosition(x,   y,   z+1),
                        _data.getPointPosition(x,   y+1, z),
                        _data.getPointPosition(x+1, y+1, z),
                        _data.getPointPosition(x+1, y+1, z+1),
                        _data.getPointPosition(x,   y+1, z+1)
                    };

                    int index = 0;
                    for (int i = 0; i < 8; i++) {
                        if (cubeValues[i]) {
                            index += 1 << i;
                        }
                    }

                    int[] midpointIndexes = TriangulationHelper.TriangulationTable[index];

                    if (midpointIndexes.Length > 0) {
                        List<int> list = new List<int>();
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
                }
            }
        }
    }

    public IEnumerator MarchingCubesCoroutine() {
        int midpointVertCount = 0;

        yield return new WaitForSeconds(1f);

        for (int y = -1; y < _data.numPointsAlongAxis; y++) {
            for (int z = -1; z < _data.numPointsAlongAxis; z++) {
                for (int x = -1; x < _data.numPointsAlongAxis; x++) {
                    _x = x;
                    _y = y;
                    _z = z;

                    bool[] cubeValues = {
                        _data.getPointValue(x,   y,   z),
                        _data.getPointValue(x+1, y,   z),
                        _data.getPointValue(x+1, y,   z+1),
                        _data.getPointValue(x,   y,   z+1),
                        _data.getPointValue(x,   y+1, z),
                        _data.getPointValue(x+1, y+1, z),
                        _data.getPointValue(x+1, y+1, z+1),
                        _data.getPointValue(x,   y+1, z+1)
                    };

                    Vector3[] cubePositions = {
                        _data.getPointPosition(x,   y,   z),
                        _data.getPointPosition(x+1, y,   z),
                        _data.getPointPosition(x+1, y,   z+1),
                        _data.getPointPosition(x,   y,   z+1),
                        _data.getPointPosition(x,   y+1, z),
                        _data.getPointPosition(x+1, y+1, z),
                        _data.getPointPosition(x+1, y+1, z+1),
                        _data.getPointPosition(x,   y+1, z+1)
                    };

                    int index = 0;
                    for (int i = 0; i < 8; i++) {
                        if (cubeValues[i]) {
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

                    yield return new WaitForSeconds(0.15f);
                }
            }
        }

        yield return null;
    }
}
