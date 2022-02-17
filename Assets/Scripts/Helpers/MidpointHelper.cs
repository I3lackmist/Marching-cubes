using System;
using System.Collections.Generic;
using UnityEngine;
public class MidpointHelper {
    public static Vector3[] GetConnectionVertexes(int[] connections, Vector3[] cube) {
        Vector3[] result = new Vector3[connections.Length];

        for (int i = 0; i < connections.Length; i++) {
            switch(i) {
                case 0:
                    result[i] = (cube[0] + cube[1]) / 2;
                    break;
                case 1:
                    result[i] = (cube[1] + cube[2]) / 2;
                    break;
                case 2:
                    result[i] = (cube[2] + cube[3]) / 2;
                    break;
                case 3:
                    result[i] = (cube[3] + cube[0]) / 2;
                    break;
                case 4:
                    result[i] = (cube[4] + cube[5]) / 2;
                    break;
                case 5:
                    result[i] = (cube[5] + cube[6]) / 2;
                    break;
                case 6:
                    result[i] = (cube[6] + cube[7]) / 2;
                    break;
                case 7:
                    result[i] = (cube[7] + cube[4]) / 2;
                    break;
                case 8:
                    result[i] = (cube[4] + cube[0]) / 2;
                    break;
                case 9:
                    result[i] = (cube[5] + cube[1]) / 2;
                    break;
                case 10:
                    result[i] = (cube[6] + cube[2]) / 2;
                    break;
                case 11:
                    result[i] = (cube[7] + cube[3]) / 2;
                    break;
                default:
                    break;
            }
        }

        return result;
    }
}
