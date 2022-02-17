using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchingCubeField : MonoBehaviour {

    private MarchingCubeFieldData _data = new MarchingCubeFieldData();
    private Mesh _mesh;
    private MeshFilter _meshFilter;

    private float? _distanceBetweenPoints;
    private Vector3? _fieldCenter;
    private int? _numPointsAlongAxis;
    public float DistanceBetweenPoints {
        get {
            return _distanceBetweenPoints ?? _data.distanceBetweenPoints;
        }
        set {
            _distanceBetweenPoints = value;
        }
    }
    
    public Vector3 FieldCenter {
        get {
            return _fieldCenter ?? _data.fieldCenter;
        }
        set {
            _fieldCenter = value;
        }
    }
    
    public int NumPointsAlongAxis {
        get {
            return _numPointsAlongAxis ?? _data.numPointsAlongAxis;
        }
        set {
            _numPointsAlongAxis = value;
        }
    }

    public Vector3 GetPointPosition(int x, int y, int z) {
        return _data.getPointPosition(x, y, z);
    }

    public bool GetPointValue(int x, int y, int z) {
        return _data.getPointValue(x, y, z);
    }

    public void RegenerateValues() {
        _data.distanceBetweenPoints = DistanceBetweenPoints;
        _data.fieldCenter = FieldCenter;
        _data.numPointsAlongAxis = NumPointsAlongAxis;

        _data.generateValues();
        
        _meshFilter.mesh = null;
    }

    public void GenerateMesh() {
        MarchingCubes();
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_data.fieldCenter, 0.25f);

        for (int x = 0; x < NumPointsAlongAxis; x++) {
            for (int y = 0; y < NumPointsAlongAxis; y++) {
                for (int z = 0; z < NumPointsAlongAxis; z++) {
                    Gizmos.color = GetPointValue(x, y, z) ? Color.white : Color.black;
                    Gizmos.DrawSphere(GetPointPosition(x, y, z), 0.1f);
                }
            }
        }
    }

    public void MarchingCubes() {
        _mesh = new Mesh();

        List<Vector3> newVerts = new List<Vector3>();
        List<int> newTris = new List<int>();
        
        int vertCount = 0;

        for (int x = 0; x < NumPointsAlongAxis - 1; x++) {
            for (int y = 0; y < NumPointsAlongAxis - 1; y++) {
                for (int z = 0; z < NumPointsAlongAxis - 1; z++) {
                    bool[] cubeValues = {
                        GetPointValue(x,   y,   z),
                        GetPointValue(x+1, y,   z),
                        GetPointValue(x+1, y,   z+1),
                        GetPointValue(x,   y,   z+1),
                        GetPointValue(x,   y+1, z),
                        GetPointValue(x+1, y+1, z+1),
                        GetPointValue(x+1, y+1, z+1),
                        GetPointValue(x,   y+1, z+1)
                    };

                    Vector3[] cubePositions = {
                        GetPointPosition(x,   y,   z),
                        GetPointPosition(x+1, y,   z),
                        GetPointPosition(x+1, y,   z+1),
                        GetPointPosition(x,   y,   z+1),
                        GetPointPosition(x,   y+1, z),
                        GetPointPosition(x+1, y+1, z+1),
                        GetPointPosition(x+1, y+1, z+1),
                        GetPointPosition(x,   y+1, z+1)
                    };

                    int index = 0;
                    for (int i = 0; i < 8; i++) {
                        if (cubeValues[i]) {
                            index += 1 << i;
                        }
                    }

                    int[] connections = TriangulationTable.TriangleConnectionTable[index];
                    
                    if (connections.Length > 0) {
                        Vector3[] connectionVertexes = MidpointHelper.GetConnectionVertexes(connections, cubePositions);
                        newVerts.AddRange(connectionVertexes);

                        int[] tris = new int[connections.Length];
                        for (int i = 0; i< connections.Length; i++) {
                            tris[i] = vertCount + i;
                        }
                        newTris.AddRange(tris);

                        vertCount += connections.Length;
                    }
                }
            }
        }

        _mesh.vertices = newVerts.ToArray();
        _mesh.triangles = newTris.ToArray();

        _mesh.RecalculateNormals();
        _mesh.RecalculateBounds();

        _meshFilter = gameObject.GetComponent<MeshFilter>();
        _meshFilter.mesh = _mesh;
    }
}
