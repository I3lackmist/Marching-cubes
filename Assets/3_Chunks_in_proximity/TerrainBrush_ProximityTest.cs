using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainBrush_ProximityTest : MonoBehaviour
{
	private int _size;
	public int Size {
		get {
			return _size;
		}
		set {
			_size = value;
			MakeBrushArea();
		}
	}

	public List<Vector2Int> _brush;

	public List<Vector2Int> Brush {
		get {
			return _brush;
		}
	}

	public void MakeBrushArea() {
		_brush = new List<Vector2Int>();

		// for (int r = 0; r < Size; r++) {
		// 	BresenhamCircle(r);
		// }

		Circle(Size);

		_brush.Sort();
	}

	public void Circle(int radius) {
		for(int y=-radius; y<=radius; y++)
			for(int x=-radius; x<=radius; x++)
				if(x*x+y*y <= radius*radius)
					AddToBrush(new Vector2Int(x, y));
	}

	public void BresenhamCircle(int radius) {
		int x = 0;
		int y = radius;
		int m = 5 - (radius<<2);
		while ( x<=y ) {
			AddToBrush(new Vector2Int(x,y));
			AddToBrush(new Vector2Int(x,-y));
			AddToBrush(new Vector2Int(-x,y));
			AddToBrush(new Vector2Int(-x,-y));
			AddToBrush(new Vector2Int(y,x));
			AddToBrush(new Vector2Int(y,-x));
			AddToBrush(new Vector2Int(-y,x));
			AddToBrush(new Vector2Int(-y,-x));

			if (m > 0) {
				y--;
				m-= (y<<3);
			}

			x++;
			m += (y<<3) + 4;
		}
	}

	public void Line(Vector2Int from, Vector2Int to) {

	}

	public void AddToBrush(Vector2Int point) {
		if (!_brush.Contains(point)) {
			_brush.Add(point);
		}
	}
}