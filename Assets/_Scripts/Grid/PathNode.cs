using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
	GridCell cell;
	PathNode previous;
	float gCost; // The distance in number of cells from the first node in a path (including obstacles)
	float hCost; // The Euclidean distance to the last cell in a path (excluding obstacles)

	public GridCell Cell { get { return cell; } set { cell = value; } }
	public PathNode Previous { get { return previous; } set { previous = value; } }
	public float GCost { get { return gCost; } set { gCost = value; } }
	public float HCost { get { return hCost; } set { hCost = value; } }

	public PathNode(GridCell gridCell, PathNode previousNode)
	{
		cell = gridCell;
		previous = previousNode;
	}

	// Get the combined cost to find the shortest path
	public float GetFCost() { return GCost + HCost; }
}
