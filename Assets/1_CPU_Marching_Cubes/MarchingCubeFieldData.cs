using UnityEngine;

[System.Serializable]
public class MarchingCubeFieldData {
    public float distanceBetweenPoints;
    public int numPointsAlongAxis;
    private float[] pointValues;
    
    public void generateRandomValues() {
        System.Random rnd = new System.Random();
        pointValues = new float[numPointsAlongAxis * numPointsAlongAxis * numPointsAlongAxis];

        for (int x = 0; x < numPointsAlongAxis; x++) {
            for (int y = 0; y < numPointsAlongAxis; y++) {
                for (int z = 0; z < numPointsAlongAxis; z++) {
                    pointValues[IndexFromCoordinates(x,y,z)] =  rnd.Next(0,100) / 100f;
                }
            }
        }
    }

    public void generateCubeValues() {
        pointValues = new float[numPointsAlongAxis * numPointsAlongAxis * numPointsAlongAxis];

        for (int x = 0; x < numPointsAlongAxis; x++) {
            for (int y = 0; y < numPointsAlongAxis; y++) {
                for (int z = 0; z < numPointsAlongAxis; z++) {
                    float value = 0;

                    if (
                        x == 0 || x == numPointsAlongAxis-1 ||
                        y == 0 || y == numPointsAlongAxis-1 ||
                        z == 0 || z == numPointsAlongAxis-1
                    )  value = 0;
                    else value = 1;
                    
                    pointValues[IndexFromCoordinates(x,y,z)] = value;
                }
            }
        }
    }

    public int IndexFromCoordinates(int x, int y, int z) {
        return z*numPointsAlongAxis*numPointsAlongAxis + y*numPointsAlongAxis + x;
    }

    public Vector3 getPointPosition(Vector3Int index) {
        validateIndex(index);

        return
            new Vector3(index.x, index.y, index.z) * distanceBetweenPoints;
    }

    public float getPointValue(Vector3Int index) {
        validateIndex(index);

        return pointValues[(index.z * numPointsAlongAxis * numPointsAlongAxis) + (index.y * numPointsAlongAxis) + index.x];
    }

    private void validateIndex(Vector3Int index) {
        if (
            index.x < 0 || index.x > numPointsAlongAxis ||
            index.y < 0 || index.y > numPointsAlongAxis ||
            index.z < 0 || index.z > numPointsAlongAxis
        )  {
            throw new System.IndexOutOfRangeException($"Accessing point at index out of bounds. Value: {index}");
        }
    }
}
