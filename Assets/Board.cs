using System.Collections.Generic;

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
                lCells[i] = cells[(i, j)];
            }
            hLines[i] = new Line(lCells, hClues[i]);
        }

        for (var i = 0; i < cols; i++)
        {
            Cell[] lCells = new Cell[rows];
            for (var j = 0; j < rows; j++)
            {
                lCells[i] = cells[(j, i)];
            }
            vLines[i] = new Line(lCells, vClues[i]);
        }
    }
    
    public void Solve()
    {
        // TODO : implement the thing
    }
}
