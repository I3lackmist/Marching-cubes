using System;

namespace MarchingCubes.Common.Interfaces 
{
	public interface IRenderable
	{
		void Render(Action doneFunction);
		bool Visible { get; }
	}
}
