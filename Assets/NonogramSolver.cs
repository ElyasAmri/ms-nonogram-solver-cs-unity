using System.Collections.Generic;
using UnityEngine;

public class NonogramSolver : MonoBehaviour
{
    
    [InspectorName("Horizontal Clues"), SerializeField] List<ClueLine> hClues;
    [InspectorName("Vertical Clues"), SerializeField] List<ClueLine> vClues;

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
                Gizmos.DrawWireCube(new Vector3(- x - offsetX - 1, i - offsetY), small);
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
}
