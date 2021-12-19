using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
            foreach (var i in nextH)
            {
                newNextV.AddRange(hLines[i].Resolve());
            }

            foreach (var i in nextV)
            {
                newNextH.AddRange(vLines[i].Resolve());
            }

            nextH.Clear();
            nextV.Clear();
            nextH.AddRange(newNextH);
            nextV.AddRange(newNextV);
            newNextH.Clear();
            newNextV.Clear();
        } while (nextH.Count != 0 || nextV.Count != 0);
        // Basically, while there is still something to check

        Debug.Log($"Loops took: {loops}");
    }

    public Dictionary<(int, int), Cell> GetResult()
    {
        return cells;
    }
}
