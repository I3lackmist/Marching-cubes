using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MarchingCubeFieldData {
    public float distanceBetweenPoints;
    public Vector3 fieldCenter;
    public int numPointsAlongAxis;

    private bool[] pointValues;

    public MarchingCubeFieldData() {
        distanceBetweenPoints = 0.25f;
        fieldCenter = Vector3.zero;
        numPointsAlongAxis = 15;

        generateValues();
    }

    public void generateValues() {
        System.Random rnd = new System.Random();
        pointValues = new bool[(int)Mathf.Pow(numPointsAlongAxis, 3)];

        for (int x = 0; x < numPointsAlongAxis; x++) {
            for (int y = 0; y < numPointsAlongAxis; y++) {
                for (int z = 0; z < numPointsAlongAxis; z++) {
                    pointValues[(z * numPointsAlongAxis * numPointsAlongAxis) + (y * numPointsAlongAxis) + x] = ( rnd.Next(0, 100) > 50) ? true : false;
                }
            }
        }
    }

    public Vector3 getPointPosition(int x, int y, int z) {
        return 
            fieldCenter -
            Vector3.one * ((float)numPointsAlongAxis * distanceBetweenPoints) / 2f + 
            new Vector3(x, y, z) * distanceBetweenPoints;
    }

    public bool getPointValue(int x, int y, int z) {
        return pointValues[(z * numPointsAlongAxis * numPointsAlongAxis) + (y * numPointsAlongAxis) + x];
    }

    public void togglePointValue(int x, int y, int z) {
        pointValues[(z * numPointsAlongAxis * numPointsAlongAxis) + (y * numPointsAlongAxis) + x] = !pointValues[(z * numPointsAlongAxis * numPointsAlongAxis) + (y * numPointsAlongAxis) + x];
    }
}
