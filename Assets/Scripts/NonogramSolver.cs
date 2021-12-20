using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NonogramSolver : MonoBehaviour
{
	[InspectorName("Horizontal Clues"), SerializeField]
	List<ClueLine> hClues;

	[InspectorName("Vertical Clues"), SerializeField]
	List<ClueLine> vClues;

	[Header("Visualization")] [SerializeField]
	GameObject fullBlock;

	[SerializeField] GameObject emptyBlock;
	[SerializeField] GameObject unknownBlock;
	[SerializeField] GameObject clueBlock;

	[Header("Step Solving")] [SerializeField]
	bool stepSolvingEnabled = false;

	[SerializeField] float frameTime = 0.25f;

	float rows;
	float cols;
	float offsetX;
	float offsetY;

	void Awake()
	{
		rows = hClues.Count;
		cols = vClues.Count;
		offsetX = cols / 2f - 0.5F;
		offsetY = rows / 2f - 0.5F;
	}

	void OnDrawGizmos()
	{
		Vector3 min = Vector3.one * 0.9f;
		Vector3 small = Vector3.one * 0.8f;

		for (var i = 0; i < rows; i++)
		{
			for (var j = 0; j < cols; j++)
				Gizmos.DrawWireCube(new Vector3(j - offsetX, i - offsetY), min);
			for (var x = 0; x < hClues[i].values.Count; x++)
			{
				Gizmos.DrawWireCube(new Vector3(-x - offsetX - 1, rows - i - 1 - offsetY), small);
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
		DrawCanvas();
		if (stepSolvingEnabled)
		{
			StartCoroutine(MultiStepSolve());
		}
		else
		{
			OneStepSolve();
		}
	}

	IEnumerator MultiStepSolve()
	{
		Board board = new Board(hClues, vClues);
		var waitTime = new WaitForSeconds(frameTime);
		foreach (var newCells in board.SolveSteps())
		{
			foreach (var (i, j, s) in newCells)
			{
				CreateBlock(new Vector3(j - offsetX, rows - i - 1 - offsetY), GetTypeFromBool(s));
				yield return waitTime;
			}
		}
	}

	void OneStepSolve()
	{
		Board board = new Board(hClues, vClues);
		board.Solve();
		var result = board.GetResult();

		for (var i = 0; i < rows; i++)
		{
			for (var j = 0; j < cols; j++)
				CreateBlock(new Vector3(j - offsetX, rows - i - 1 - offsetY), GetTypeFromBool(result[(i, j)].value));
		}
	}

	void DrawCanvas()
	{
		for (var i = 0; i < rows; i++)
		{
			for (var x = 0; x < hClues[i].values.Count; x++)
			{
				CreateClue(new Vector3(-x - offsetX - 1, rows - i - 1 - offsetY), hClues[i].values[x]);
			}
		}

		for (var i = 0; i < cols; i++)
		{
			for (var y = 0; y < vClues[i].values.Count; y++)
			{
				CreateClue(new Vector3(i - offsetX, y - offsetY + rows), vClues[i].values[y]);
			}
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

	void CreateClue(Vector3 position, int value)
	{
		Instantiate(clueBlock, position, Quaternion.identity, transform)
			.GetComponentInChildren<Text>().text = value.ToString();
	}
}

public enum BlockType
{
	Clue,
	Full,
	None,
	Unknown,
}