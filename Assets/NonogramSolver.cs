using System;
using System.Collections.Generic;
using UnityEngine;

public class NonogramSolver : MonoBehaviour
{
    
    [InspectorName("Horizontal Clues"), SerializeField] List<ClueLine> hClues;
    [InspectorName("Vertical Clues"), SerializeField] List<ClueLine> vClues;

    [Header("Visualization")]
    [SerializeField] GameObject fullBlock;
    [SerializeField] GameObject emptyBlock;
    [SerializeField] GameObject unknownBlock;
    [SerializeField] GameObject clueBlock;

    void OnDrawGizmos()
    {
        var rows = hClues.Count;
        var cols = vClues.Count;
        var offsetX = cols / 2f - 0.5F;
        var offsetY = rows / 2f - 0.5F;

        
        Vector3 min = Vector3.one * 0.9f; 
        Vector3 small = Vector3.one * 0.8f; 
        
        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
                Gizmos.DrawWireCube(new Vector3(j - offsetX, i - offsetY), min);
            for (var x = 0; x < hClues[i].values.Count; x++)
            {
                Gizmos.DrawWireCube(new Vector3(- x - offsetX - 1, rows - i - 1 - offsetY), small);
            }
        }

        for (var i = 0; i < cols; i++)
        {
            for (var y = 0; y < vClues[i].values.Count; y++)
            {
                Gizmos.DrawWireCube(new Vector3(i - offsetX, y - offsetY + rows), small);
            }
        }
    }

    void Start()
    {
        Board board = new Board(hClues, vClues);
        board.Solve();
        var result = board.GetResult();
        // TODO: print the result
        var rows = hClues.Count;
        var cols = vClues.Count;
        var offsetX = cols / 2f - 0.5F;
        var offsetY = rows / 2f - 0.5F;

        for (var i = 0; i < rows; i++)
        {
            for (var j = 0; j < cols; j++)
                CreateBlock(new Vector3(j - offsetX, i - offsetY), GetTypeFromBool(result[(i, j)].value));
            for (var x = 0; x < hClues[i].values.Count; x++)
            {
                CreateBlock(new Vector3(- x - offsetX - 1, rows - i - 1 - offsetY), BlockType.Clue);
            }
        }

        for (var i = 0; i < cols; i++)
        {
            for (var y = 0; y < vClues[i].values.Count; y++)
            {
                CreateBlock(new Vector3(i - offsetX, y - offsetY + rows), BlockType.Clue);
            }
        }

        BlockType GetTypeFromBool(bool? b)
        {
            return b switch
            {
                true => BlockType.Full,
                false => BlockType.None,
                null => BlockType.Unknown
            };
        }
        
        void CreateBlock(Vector3 position, BlockType type)
        {
            Instantiate(type switch
                {
                    BlockType.Clue => clueBlock,
                    BlockType.Full => fullBlock,
                    BlockType.None => emptyBlock,
                    BlockType.Unknown => unknownBlock,
                    _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
                }, position, Quaternion.identity, transform);
        }
    }
}

public enum BlockType
{
    Clue, Full, None, Unknown,
}