using System.Linq;
using UnityEngine;
using MarchingCubes.Common.Structs;

namespace MarchingCubes.Common.Helpers {
    public static class DebugHelper
    {
        public static void debugTriangles(ResultTriangle[] resultTriangles) {
            Debug.Log(
                string.Join(
                    ' ', 
                    resultTriangles.Select(
                        tri => string.Join(',', tri.vertexA, tri.vertexB, tri.vertexC)
                    )
                )
            );
        }
    }
}