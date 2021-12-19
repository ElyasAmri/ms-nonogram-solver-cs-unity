using System;
using System.Collections.Generic;
using System.Linq;

public class Line
{
	Cell[] cells;
	int length;

	List<bool[]> possibilities;

	public Line(Cell[] cells, ClueLine clues)
	{
		this.cells = cells;
		length = cells.Length;

		possibilities = Utils.GeneratePermutations(clues, length);
		possibilities.RemoveAll(poss =>
		{
			var index = 0;
			foreach (var clue in clues.values)
			{
				// Skip till the first true
				for (; index < length && !poss[index]; index++) ;
				
				var capture = 1;
				index++;
				
				// Skip till the first false
				for (; index < length && poss[index]; index++, capture++) ;
				
				// Either bigger or smaller than intended
				if (capture != clue)
					return true;
			}

			return false;
		});
	}

	public int[] Resolve()
	{
		List<int> ret = new List<int>();

		// A simple measure to skip rolling all of the cells for nothing
		if (!cells.Any(cell => cell.value.HasValue))
			goto skipRemoval;
		
		// Remove failed possibilities
		possibilities.RemoveAll(poss =>
		{
			for (var i = 0; i < length; i++)
			{
				// if the cell has been determined and it doesn't match its
				// respective position in the possibility then remove it
				if (cells[i].value.HasValue && poss[i] != cells[i].value.Value)
					return true;
			}
			return false;
		});
		
		skipRemoval: ;
		
		// Determine if there is a new cell after the removal
		for (var i = 0; i < length; i++)
		{
			// ignore already calculated cells
			if(cells[i].value.HasValue) 
				continue;
			
			// check if all possibilities for one cell are true
			if (possibilities.TrueForAll(poss => poss[i])) 
				cells[i].value = true;
			
			// check if all possibilities for one cell are false
			else if (possibilities.TrueForAll(poss => !poss[i]))
				cells[i].value = false;
			
			// Failed to find such case
			else continue;
			
			// Add if not failed
			ret.Add(i);
		}
		
		return ret.Count == 0 ? Array.Empty<int>() : ret.ToArray();
	}
}
