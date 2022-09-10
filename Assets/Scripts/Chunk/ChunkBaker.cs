using System;
using UnityEngine;
public static class ChunkBaker {

    private const int RESULT_TRIANGLE_STRIDE = 36;

    private static Mesh CreateMesh(ResultTriangle[] triangles) {
        Mesh mesh = new Mesh();

        Vector3[] verts = new Vector3[triangles.Length*3];
        int[] tris = new int[triangles.Length*3];

        int vertIndex = 0;
		int idx = 0;

        foreach(ResultTriangle triangle in triangles) {
            verts[idx++] = triangle.vertexA;
            verts[idx++] = triangle.vertexB;
            verts[idx++] = triangle.vertexC;

			for (int i = 0; i < 3; i++) tris[vertIndex] = vertIndex++;
        }

        mesh.vertices = verts;
        mesh.triangles = tris;

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        mesh.Optimize();

        return mesh;
    }

    public static void BakeChunkMesh(Chunk chunk, Action<Mesh> setMeshCallback) {
        // Density
        GraphicsBuffer isoLevelsBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Append, 512, 4);

        chunk.volumeProperties.densityShader.SetBuffer(0, "isoValues", isoLevelsBuffer);

		chunk.volumeProperties.densityShader.Dispatch(0, 1, 1, 1);

		// Marching cubes
		int totalCubes = (int)Mathf.Pow(7, 3);
        int maxTriangles = totalCubes * 5;

        GraphicsBuffer resultTrianglesBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Append, maxTriangles, RESULT_TRIANGLE_STRIDE);

		chunk.volumeProperties.marchingCubesShader.SetFloat("isoLevel", chunk.volumeProperties.isoLevel);

        chunk.volumeProperties.marchingCubesShader.SetFloat("distanceBetweenPoints", chunk.volumeProperties.distanceBetweenPoints);

        chunk.volumeProperties.marchingCubesShader.SetInts("chunkIndex", new int[3] {
            chunk.chunkProperties.index.x,
            chunk.chunkProperties.index.y,
            chunk.chunkProperties.index.z
        });

        chunk.volumeProperties.marchingCubesShader.SetFloats("chunkOrigin", new float[3] {
			chunk.chunkProperties.origin.x,
			chunk.chunkProperties.origin.y,
			chunk.chunkProperties.origin.z
		});

        chunk.volumeProperties.marchingCubesShader.SetBuffer(0, "resultTriangles", resultTrianglesBuffer);
        chunk.volumeProperties.marchingCubesShader.SetBuffer(0, "isoValues", isoLevelsBuffer);

        chunk.volumeProperties.marchingCubesShader.Dispatch(0, 1, 1, 1);

		ResultTriangle[] resultTriangles = new ResultTriangle[resultTrianglesBuffer.count];

		resultTrianglesBuffer.GetData(resultTriangles);

		isoLevelsBuffer.Release();
		resultTrianglesBuffer.Release();

		Mesh resultMesh = CreateMesh(resultTriangles);
		setMeshCallback(resultMesh);
    }
}
