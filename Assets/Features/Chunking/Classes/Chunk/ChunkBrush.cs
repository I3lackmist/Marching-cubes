using System.Collections.Generic;
using UnityEngine;

namespace MarchingCubes.Chunking.Classes 
{
	public class ChunkBrush
	{
		private int size;

		public int Size {
			get {
				return size;
			}
			set {
				size = value;

				CreateBrushVolume();
			}
		}

		public List<Vector3Int> Brush;

		public void CircleBrush(int radius) {
			int diameter = radius*radius;

			for(int z = -radius; z <= radius; z++)
				for(int x = -radius; x <= radius; x++)
					for(int y = -radius; y <= radius; y++)
						if(x*x + z*z + y*y <= diameter)
							AddToBrush(new Vector3Int(x, y, z));
		}

		private void AddToBrush(Vector3Int point) {
			if (!Brush.Contains(point)) {
				Brush.Add(point);
			}
		}

		private void CreateBrushVolume() {
			Brush = new List<Vector3Int>();

			CircleBrush(size);
		}
	}
}