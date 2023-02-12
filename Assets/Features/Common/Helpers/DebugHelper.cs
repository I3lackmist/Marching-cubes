using System.Linq;
using MarchingCubes.Common.Structs;
using UnityEngine;

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