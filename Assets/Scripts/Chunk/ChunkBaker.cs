using System.Collections.Generic;
using UnityEngine;
public static class ChunkBaker {
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    private struct ResultTriangle {
        public Vector3 vertexA;
        public Vector3 vertexB;
        public Vector3 vertexC;
    }
    private const int RESULT_TRIANGLE_STRIDE = 36;

    private static Mesh CreateMesh(ResultTriangle[] triangles) {
        Mesh mesh = new Mesh();

        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();

        int vertIndex = 0;

        foreach(ResultTriangle triangle in triangles) {
            verts.Add(triangle.vertexA);
            verts.Add(triangle.vertexB);
            verts.Add(triangle.vertexC);

            tris.Add(vertIndex++);
            tris.Add(vertIndex++);
            tris.Add(vertIndex++);
        }
        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        // mesh.Optimize();

        return mesh;
    }

    public static Mesh BakeChunkMesh(ComputeShader shader, ChunkOptions options) {
        int kernelId = shader.FindKernel("MarchingCubes");

        int totalCubes = (int)Mathf.Pow(options.pointsPerAxis-1, 3);
        int maxTriangles = totalCubes * 5;

        GraphicsBuffer resultTrianglesBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Append, maxTriangles, RESULT_TRIANGLE_STRIDE);

        shader.SetFloat("distanceBetweenPoints", options.distanceBetweenPoints);

        shader.SetFloats("chunkOrigin", new float[3] {
                options.chunkOrigin.x,
                options.chunkOrigin.y,
                options.chunkOrigin.z
            }
        );

        shader.SetBuffer(kernelId, "resultTriangles", resultTrianglesBuffer);

        shader.Dispatch(kernelId, 1, 1, 1);
        ResultTriangle[] resultTriangles = new ResultTriangle[resultTrianglesBuffer.count];

        resultTrianglesBuffer.GetData(resultTriangles);

        resultTrianglesBuffer.Release();

        Mesh resultMesh = CreateMesh(resultTriangles);

        return resultMesh;
    }
}
