using System;
using System.Collections.Generic;

public class Line
{
	ClueLine clues;
	Cell[] cells;
	int length;

	List<int[]> possibilities;

	public Line(Cell[] cells, ClueLine clues)
	{
		this.clues = clues;
		this.cells = cells;
		this.length = cells.Length;

		possibilities = new List<int[]>();
		// TODO: make permutations from a utility
	}

	public int[] Resolve()
	{
		List<int> ret = new List<int>();
		
		// Remove failed possibilities
		possibilities.RemoveAll(poss =>
		{
			// TODO: check the validity of the possibility
			return false;
		});
		
		// Determine if there is a new cell after the removal
		for (var i = 0; i < length; i++)
		{
			// ignore already calculated cells
			if(cells[i].value.HasValue) 
				continue;
			
			// check if all possibilities for one cell are true
			if (possibilities.TrueForAll(poss =>
			{
				// TODO: check for all true
				return true;
			})) 
				cells[i].value = true;
			
			// check if all possibilities for one cell are false
			else if (possibilities.TrueForAll(poss =>
			{
				// TODO: check for all false
				return true;
			}))
				cells[i].value = false;
			
			// Failed to find such case
			else continue;
			
			// Add if not failed
			ret.Add(i);
		}
		
		return ret.Count == 0 ? Array.Empty<int>() : ret.ToArray();
	}
}
