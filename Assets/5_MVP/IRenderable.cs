using System;

public interface IRenderable
{
	void Render(Action doneFunction);
	bool Visible { get; }
}
