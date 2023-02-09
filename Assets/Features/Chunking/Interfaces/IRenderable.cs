using System;

namespace MarchingCubes.Chunking.Interfaces {
	public interface IRenderable
	{
		void Render(Action doneFunction);
		bool Visible { get; }
	}
}
