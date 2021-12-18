using System.Collections.Generic;
using System.Linq;

public class Board
{
    int rows;
    int cols;

    Line[] hLines; 
    Line[] vLines;

    Dictionary<(int, int), Cell> cells;

    public Board(List<ClueLine> hClues, List<ClueLine> vClues)
    {
        rows = hClues.Count;
        cols = vClues.Count;

        hLines = new Line[rows];
        vLines = new Line[cols];

        cells = new Dictionary<(int, int), Cell>();
        
        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
            {
                cells[(i, j)] = new Cell();
            }
        }
        
        for (var i = 0; i < rows; i++)
        {
            Cell[] lCells = new Cell[cols];
            for (var j = 0; j < cols; j++)
            {
                lCells[j] = cells[(i, j)];
            }
            hLines[i] = new Line(lCells, hClues[i]);
        }

        for (var i = 0; i < cols; i++)
        {
            Cell[] lCells = new Cell[rows];
            for (var j = 0; j < rows; j++)
            {
                lCells[j] = cells[(j, i)];
            }
            vLines[i] = new Line(lCells, vClues[i]);
        }
    }
    
    public void Solve()
    {
        var nextH = Enumerable.Range(0, rows).ToList();
        var nextV = Enumerable.Range(0, cols).ToList();
        var newNextH = new List<int>();
        var newNextV = new List<int>();
        do
        {
            foreach (var i in nextH)
            {
                var res = hLines[i].Resolve();
                if (res != -1) newNextH.Add(res);
            }

            foreach (var i in nextV)
            {
                var res = vLines[i].Resolve();
                if (res != -1) newNextV.Add(res);
            }

            nextH = newNextH;
            nextV = newNextV;
            newNextH.Clear();
            newNextV.Clear();
        } while (nextH.Any() || nextV.Any());
        // Basically, while there is still something to check
    }

    public Dictionary<(int, int), Cell> GetResult()
    {
        return cells;
    }
}
