using UnityEngine;

[System.Serializable]
public class MarchingCubeFieldData {
    public float distanceBetweenPoints;
    public Vector3 fieldCenter;
    public int numPointsAlongAxis;
    public int terrainThreshold;

    private int[] pointValues;

    public MarchingCubeFieldData() {}

    public void generateValues() {
        System.Random rnd = new System.Random();
        pointValues = new int[numPointsAlongAxis * numPointsAlongAxis * numPointsAlongAxis];

        for (int x = 0; x < numPointsAlongAxis; x++) {
            for (int y = 0; y < numPointsAlongAxis; y++) {
                for (int z = 0; z < numPointsAlongAxis; z++) {
                    pointValues[(z * numPointsAlongAxis * numPointsAlongAxis) + (y * numPointsAlongAxis) + x] = Random.Range(0,101);
                }
            }
        }
    }

    public Vector3 getPointPosition(int x, int y, int z) {
        return
            fieldCenter -
            Vector3.one * (((float)numPointsAlongAxis - 1f) * distanceBetweenPoints) * 0.5f +
            new Vector3(x, y, z) * distanceBetweenPoints;
    }

    public bool getPointValue(int x, int y, int z) {
        if (x<0 || x>=numPointsAlongAxis || y<0 || y>=numPointsAlongAxis || z<0 || z>=numPointsAlongAxis) return false;

        return pointValues[(z * numPointsAlongAxis * numPointsAlongAxis) + (y * numPointsAlongAxis) + x] >= terrainThreshold;
    }
}
