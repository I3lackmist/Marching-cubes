using System.Collections.Generic;
using UnityEngine;

public class TerrainBrush
{
	private int _size;
	private bool _is3D;
	public bool Is3D {
		get {
			return _is3D;
		}
		set {
			_is3D = value;
			MakeBrushArea();
		}
	}
	public int Size {
		get {
			return _size;
		}
		set {
			_size = value;
			MakeBrushArea();
		}
	}

	public List<Vector3Int> Brush;

	public void MakeBrushArea() {
		Brush = new List<Vector3Int>();
		Circle(Size);
	}

	public void Circle(int radius) {
		int ylim = Is3D ? radius : 0;

		for(int z=-radius; z<=radius; z++)
			for(int x=-radius; x<=radius; x++)
				for(int y = Is3D ? -radius : 0; y<=ylim; y++)
					if(x*x + z*z + y*y <= radius*radius)
						AddToBrush(new Vector3Int(x, y, z));
	}

	public void BresenhamCircle(int radius) {
		int x = 0;
		int z = radius;
		int m = 5 - (radius<<2);
		while ( x<=z ) {
			AddToBrush(new Vector3Int(x,z));
			AddToBrush(new Vector3Int(x,-z));
			AddToBrush(new Vector3Int(-x,z));
			AddToBrush(new Vector3Int(-x,-z));
			AddToBrush(new Vector3Int(z,x));
			AddToBrush(new Vector3Int(z,-x));
			AddToBrush(new Vector3Int(-z,x));
			AddToBrush(new Vector3Int(-z,-x));

			if (m > 0) {
				z--;
				m-= (z<<3);
			}

			x++;
			m += (z<<3) + 4;
		}
	}

	public void AddToBrush(Vector3Int point) {
		if (!Brush.Contains(point)) {
			Brush.Add(point);
		}
	}
}