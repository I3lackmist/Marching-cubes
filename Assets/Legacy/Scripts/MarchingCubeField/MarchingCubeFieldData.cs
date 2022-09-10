using System.Linq;
using UnityEngine;

[System.Serializable]
public class MarchingCubeFieldData {
    public float distanceBetweenPoints;
    public Vector3 fieldCenter;
    public int numPointsAlongAxis;
    private float _terrainRatio;
    private float _terrainThreshold;

    public float TerrainRatio {
        get {
            return _terrainRatio;
        }
        set {
            _terrainRatio = value;
            _terrainThreshold = Mathf.RoundToInt((valueMin + valueMax) * _terrainRatio);
        }
    }

    public float valueMin, valueMax;

    private int[] pointValues;

    public MarchingCubeFieldData() {}

    public void generateValues() {
        System.Random rnd = new System.Random();
        pointValues = new int[numPointsAlongAxis * numPointsAlongAxis * numPointsAlongAxis];

        for (int x = 0; x < numPointsAlongAxis; x++) {
            for (int y = 0; y < numPointsAlongAxis; y++) {
                for (int z = 0; z < numPointsAlongAxis; z++) {
                    pointValues[(z * numPointsAlongAxis * numPointsAlongAxis) + (y * numPointsAlongAxis) + x] =  rnd.Next(0,100);
                }
            }
        }

        valueMin = pointValues.Min();
        valueMax = pointValues.Max();
    }

    public Vector3 getPointPosition(int x, int y, int z) {
        return
            fieldCenter -
            Vector3.one * (((float)numPointsAlongAxis - 1f) * distanceBetweenPoints) * 0.5f +
            new Vector3(x, y, z) * distanceBetweenPoints;
    }

    public bool getPointValue(int x, int y, int z) {
        if (x<0 || x>=numPointsAlongAxis || y<0 || y>=numPointsAlongAxis || z<0 || z>=numPointsAlongAxis) return false;

        return pointValues[(z * numPointsAlongAxis * numPointsAlongAxis) + (y * numPointsAlongAxis) + x] > _terrainThreshold;
    }
}
