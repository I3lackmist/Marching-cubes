using UnityEngine;

public static class ChunkBaker {
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    private struct ResultTriangle {
        public Vector3 vertexA;
        public Vector3 vertexB;
        public Vector3 vertexC;
    }

    private const int RESULT_TRIANGLE_STRIDE = sizeof(float) * 3 * 3;

    private static Mesh CreateMesh(ResultTriangle[] triangles) {
        Mesh mesh = new Mesh();

        return mesh;
    }

    public static Mesh BakeChunkMesh(ComputeShader shader, ChunkOptions options) {
        int kernelId = shader.FindKernel("MarchingCubes");

        GraphicsBuffer resultTrianglesBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Append, 2560, RESULT_TRIANGLE_STRIDE);
        GraphicsBuffer resultTriangleCountBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Counter, 1, sizeof(uint));

        shader.SetFloat("distanceBetweenPoints", options.distanceBetweenPoints);
        shader.SetFloats("chunkOrigin", new float[] {
                options.chunkCenter.x - 4*options.distanceBetweenPoints,
                options.chunkCenter.y - 4*options.distanceBetweenPoints,
                options.chunkCenter.z - 4*options.distanceBetweenPoints
            }
        );

        shader.SetBuffer(kernelId, "resultTriangles", resultTrianglesBuffer);
        shader.SetBuffer(kernelId, "resultTriangleCount", resultTriangleCountBuffer);

        shader.SetFloat("terrainRatio", options.terrainRatio);

        shader.Dispatch(kernelId, 8, 8, 8);

        uint[] resultTriangleCount = new uint[1];
        resultTriangleCountBuffer.GetData(resultTriangleCount);

        Debug.Log(resultTriangleCount[0]);

        ResultTriangle[] resultTriangles = new ResultTriangle[2560];
        resultTrianglesBuffer.GetData(resultTriangles);

        resultTrianglesBuffer.Release();

        return new Mesh();
    }
}
