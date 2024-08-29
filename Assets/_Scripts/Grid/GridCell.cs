using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell
{
	public Vector2Int GridPos { get; private set; }
	public Vector2 WorldPos { get; private set; }
	public bool IsBlocked { get; private set; }

	public GridCell(Vector2Int gridPos, Vector2 worldPos, bool isBlocked)
	{
		GridPos = gridPos;
		WorldPos = worldPos;
		IsBlocked = isBlocked;
	}

	public float GetDistance(GridCell other)
	{
		if (other == null) return 0;

		return Vector2.Distance(WorldPos, other.WorldPos);
	}
}
