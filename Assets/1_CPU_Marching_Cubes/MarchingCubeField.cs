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

    [SerializeField]
    [HideInInspector]
    private float terrainRatio = 0.5f;

    private int _x = 0;
    private int _y = 0;
    private int _z = 0;

    public float TerrainRatio {
        get {
            return terrainRatio;
        }
        set {
            terrainRatio = value;
            _data.TerrainRatio = terrainRatio;
            MarchingCubes();
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

    private List<Vector3> _newVerts = new List<Vector3>();
    private List<int> _newTris = new List<int>();

    public void MarchingCubes() {
        _meshFilter = gameObject.GetComponent<MeshFilter>();
        _mesh = new Mesh();

        _newTris = new List<int>();
        _newVerts = new List<Vector3>();

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
                        _newVerts.AddRange(midpointPositions);

                        for (int i = 0; i < midpointIndexes.Length; i++) {
                            _newTris.Add(i + midpointVertCount);
                        }
                    }

                    midpointVertCount += midpointIndexes.Length;

                    _mesh.vertices = _newVerts.ToArray();
                    _mesh.triangles = _newTris.ToArray();

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
                        _newVerts.AddRange(midpointPositions);

                        for (int i = 0; i < midpointIndexes.Length; i++) {
                            _newTris.Add(i + midpointVertCount);
                        }
                    }

                    midpointVertCount += midpointIndexes.Length;

                    _mesh.vertices = _newVerts.ToArray();
                    _mesh.triangles = _newTris.ToArray();

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
