using UnityEngine;

namespace MarchingCubes.Common.Helpers 
{
    public static class Vector3IntExtensions {
        public static float[] ToFloatArray(this Vector3Int vector) 
        {
            return new float[] {vector.x, vector.y, vector.z};
        }

        public static int MagnitudeComparer(Vector3Int one, Vector3Int another)
		{
			if (one.magnitude < another.magnitude) return -1;
			if (one.magnitude == another.magnitude) return 0;
			return 1;
		}
    }
}