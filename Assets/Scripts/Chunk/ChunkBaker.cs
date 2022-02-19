using UnityEngine;

public static class ChunkBaker {
    private struct ResultTriangle {
        public Vector3 worldSpacePosition;
        public Vector3Int indexes;
    }

    private const int RESULT_TRIANGLE_STRIDE = sizeof(float) * 3 + sizeof(int) * 3;

    private static Mesh CreateMesh(ResultTriangle[] triangles) {
        Mesh mesh = new Mesh();

        return mesh;
    }

    public static Mesh BakeChunkMesh(ComputeShader shader, Vector3 chunkOrigin) {
        return new Mesh();
    }
}
