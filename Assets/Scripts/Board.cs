using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Board
{
	int rows;
	int cols;

	Line[] hLines;
	Line[] vLines;

	public Cell[,] cells { get; }

	public Board(List<ClueLine> hClues, List<ClueLine> vClues)
	{
		rows = hClues.Count;
		cols = vClues.Count;

		hLines = new Line[rows];
		vLines = new Line[cols];

		cells = new Cell[rows, cols];

		for (var i = 0; i < rows; i++)
		{
			for (var j = 0; j < cols; j++)
			{
				cells[i, j] = new Cell();
			}
		}

		for (var i = 0; i < rows; i++)
		{
			Cell[] lCells = new Cell[cols];
			for (var j = 0; j < cols; j++)
			{
				lCells[j] = cells[i, j];
			}

			hLines[i] = new Line(lCells, hClues[i]);
		}

		for (var i = 0; i < cols; i++)
		{
			Cell[] lCells = new Cell[rows];
			for (var j = 0; j < rows; j++)
			{
				lCells[j] = cells[j, i];
			}

			vLines[i] = new Line(lCells, vClues[i]);
		}
	}

	public void Solve()
	{
		var nextH = new List<int>(Enumerable.Range(0, rows));
		var nextV = new List<int>(Enumerable.Range(0, cols));
		var newNextH = new List<int>();
		var newNextV = new List<int>();

		var loops = 0;

		do
		{
			loops++;
			// Basically, everytime we fill a block, that signals
			// that the perpendicular line of what are we currently at
			// has to be checked
			foreach (var line in nextH.Select(i => hLines[i]).OrderByDescending(l => l.sum))
			{
				newNextV.AddRange(line.Resolve());
			}

			foreach (var line in nextV.Select(i => vLines[i]).OrderByDescending(l => l.sum))
			{
				newNextH.AddRange(line.Resolve());
			}

			nextH.Clear();
			nextV.Clear();
			nextH.AddRange(newNextH.Distinct());
			nextV.AddRange(newNextV.Distinct());
			newNextH.Clear();
			newNextV.Clear();
		} while (nextH.Count != 0 || nextV.Count != 0);
		// Basically, while there is still something to check

		Debug.Log($"Loops took: {loops}");
	}

	public IEnumerable<IEnumerable<(int, int, bool?)>> SolveSteps()
	{
		var nextH = new List<int>(Enumerable.Range(0, rows));
		var nextV = new List<int>(Enumerable.Range(0, cols));
		var newNextH = new List<int>();
		var newNextV = new List<int>();

		do
		{
			// Basically, everytime we fill a block, that signals
			// that the perpendicular line of what are we currently at
			// has to be checked
			foreach (var (l, i) in nextH.Select(i => (hLines[i], i)).OrderByDescending(l => l.Item1.sum))
			{
				var res = l.Resolve();
				newNextV.AddRange(res);
				if (res.Length > 0)
					yield return res.Select(y => (i, y, l.cells[y].value));
			}

			foreach (var (l, i) in nextV.Select(i => (vLines[i], i)).OrderByDescending(l => l.Item1.sum))
			{
				var res = l.Resolve();
				newNextH.AddRange(res);
				if (res.Length > 0)
					yield return res.Select(x => (x, i, l.cells[x].value));
			}

			nextH.Clear();
			nextV.Clear();
			nextH.AddRange(newNextH.Distinct());
			nextV.AddRange(newNextV.Distinct());
			newNextH.Clear();
			newNextV.Clear();
		} while (nextH.Count != 0 || nextV.Count != 0);
	}
}