using System;

public class Line
{
	public ClueLine clues;
	public Cell[] cells;

	public Line(Cell[] cells, ClueLine clues)
	{
		this.clues = clues;
		this.cells = cells;
	}

	public int Resolve()
	{
		// TODO: implement this
		return -1;
	}
}
