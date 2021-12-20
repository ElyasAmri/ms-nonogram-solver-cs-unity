using System.Collections.Generic;
using System.Linq;

public static class Utils
{
	public static List<bool[]> GeneratePermutations(ClueLine clues, int length)
	{
		var sum = clues.values.Sum();
		List<bool> list = Enumerable.Repeat(true, sum).ToList();
		list.AddRange(Enumerable.Repeat(false, length - sum));
		List<bool[]> ret = new List<bool[]>();
		do
		{
			ret.Add(list.ToArray());
		} while (NextPermutation(list));

		return ret;
	}

	static bool NextPermutation(List<bool> list)
	{
		for (var i = 0; i < list.Count - 1; i++)
		{
			if (!list[i] || list[i + 1])
				continue;

			(list[i], list[i + 1]) = (list[i + 1], list[i]);
			list.Reverse(0, i);
			return true;
		}

		return false;
	}
}