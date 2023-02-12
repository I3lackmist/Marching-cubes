using UnityEngine;

namespace MarchingCubes.Common.Helpers 
{
    public static class GridHelper
    {
        public static Vector3Int ChunkIndexFromPosition(Vector3 position, float gridCellSize) 
        {
            return new Vector3Int(
                Mathf.FloorToInt(position.x / gridCellSize),
                Mathf.FloorToInt(position.y / gridCellSize),
                Mathf.FloorToInt(position.z / gridCellSize)
            );
        }
    }
}
