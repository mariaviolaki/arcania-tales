using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(GridManager))]
public class Pathfinder : MonoBehaviour
{
	GridManager gridManager;

	void Awake()
	{
		gridManager = GetComponent<GridManager>();
	}

	public List<Vector2> GetPath(GameEnums.Scene scene, Vector2 startPos, Vector2 endPos)
	{
		if (scene == GameEnums.Scene.None) return null;

		Vector2Int startGridPos = GetGridPosFromWorldPos(scene, startPos);
		Vector2Int endGridPos = GetGridPosFromWorldPos(scene, endPos);

		GridCell startCell = gridManager.GetGridCell(scene, startGridPos.x, startGridPos.y);
		GridCell endCell = gridManager.GetGridCell(scene, endGridPos.x, endGridPos.y);

		if (startCell == null || endCell == null) return null;

		PathNode startNode = new PathNode(startCell, null);
		startNode.GCost = startNode.Cell.GetDistance(null);
		startNode.HCost = startNode.Cell.GetDistance(endCell);

		List<PathNode> toExplore = new List<PathNode>() { startNode };
		List<PathNode> explored = new List<PathNode>();

		PathNode currentNode;

		while (toExplore.Count > 0)
		{
			currentNode = GetLowestCostNode(toExplore);

			explored.Add(currentNode);
			toExplore.Remove(currentNode);

			if (currentNode.Cell == endCell)
			{
				return GetPathToNode(currentNode);
			}

			foreach (GridCell neighborCell in GetNeighborCells(scene, currentNode, explored))
			{
				ProcessNeighborCell(currentNode, neighborCell, toExplore, endCell);
			}
		}

		// The end cell wasn't found - there are no paths leading to the target position
		return null;
	}

	PathNode GetLowestCostNode(List<PathNode> pathNodes)
	{
		PathNode lowestCostNode = pathNodes[0];

		for (int i = 1; i < pathNodes.Count; i++)
		{
			PathNode node = pathNodes[i];
			if (node.GetFCost() < lowestCostNode.GetFCost() && node.HCost < lowestCostNode.HCost)
			{
				lowestCostNode = node;
			}
		}

		return lowestCostNode;
	}

	List<GridCell> GetNeighborCells(GameEnums.Scene scene, PathNode node, List<PathNode> explored)
	{
		List<GridCell> neighborCells = new List<GridCell>();

		for (int x = -1; x <= 1; x++)
		{
			for (int y = -1; y <= 1; y++)
			{
				// Don't process the current node again
				if (x == 0 && y == 0) continue;

				// Valid neighbors are only walkable cells that haven't been explored yet
				GridCell cell = gridManager.GetGridCell(scene, node.Cell.GridPos.x + x, node.Cell.GridPos.y + y);
				if (cell != null && GetCellInNodeList(cell, explored) == null)
				{
					neighborCells.Add(cell);
				}
			}
		}

		return neighborCells;
	}

	void ProcessNeighborCell(PathNode currentNode, GridCell neighborCell, List<PathNode> toExplore, GridCell endCell)
	{
		float newGCost = currentNode.GCost + currentNode.Cell.GetDistance(neighborCell);
		PathNode toExploreNode = GetCellInNodeList(neighborCell, toExplore);

		if (toExploreNode != null)
		{
			// Check if the neighbor node can obtain a lower g cost through the current node
			toExploreNode.Previous = currentNode;
			toExploreNode.GCost = newGCost;
		}
		else
		{
			// Create a new node because this neighbor isn't being explored currently
			PathNode neighborNode = new PathNode(neighborCell, currentNode);
			neighborNode.GCost = newGCost;
			neighborNode.HCost = neighborCell.GetDistance(endCell);

			toExplore.Add(neighborNode);
		}
	}

	List<Vector2> GetPathToNode(PathNode endNode)
	{
		List<Vector2> path = new List<Vector2>();
		PathNode currentNode = endNode;

		while (currentNode != null)
		{
			path.Add(currentNode.Cell.WorldPos);
			currentNode = currentNode.Previous;
		}

		path.Reverse();
		return path;
	}

	PathNode GetCellInNodeList(GridCell gridCell, List<PathNode> pathNodes)
	{
		foreach (PathNode node in pathNodes)
		{
			if (gridCell == node.Cell) return node;
		}

		return null;
	}

	Vector2Int GetGridPosFromWorldPos(GameEnums.Scene scene, Vector2 pos)
	{
		Vector2 gridPos = gridManager.GetGridPos(scene);
		Vector2 cellSize = gridManager.GetGridCellSize(scene);

		float xOffset = pos.x - gridPos.x;
		int x = (int)(Mathf.Floor(xOffset) / cellSize.x);

		float yOffset = pos.y - gridPos.y;
		int y = (int)(Mathf.Floor(yOffset) / cellSize.y);

		return new Vector2Int(x, y);
	}
}
