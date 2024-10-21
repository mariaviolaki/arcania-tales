using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridData
{
	public GameEnums.Scene Scene;
	public GridCell[,] Grid;
	public Vector2 Pos;
	public Vector2 Size;
	public Vector2 CellSize;
}

public class TerrainGrid : MonoBehaviour
{
	[Tooltip("The base tilemap above which all the others are placed")]
	[SerializeField] GameEnums.Scene scene;
	[SerializeField] Tilemap tilemap;
	[SerializeField] PolygonCollider2D gridCollider;

	Vector2 tilemapPos;
	Vector2 tilemapSize;
	Vector2 tilemapCellSize;
	GridCell[,] grid;

	public GameEnums.Scene Scene { get { return scene; } }
	public Vector2 Pos { get { return tilemapPos; } }
	public Vector2 Size { get { return tilemapSize; } }
	public Vector2 CellSize { get { return tilemapCellSize; } }
	public GridCell[,] Grid { get { return grid; } }
	public PolygonCollider2D GridCollider { get { return gridCollider; } }

	public GridData GetGridData()
	{
		return new GridData
		{
			Scene = scene,
			Grid = grid,
			Pos = tilemapPos,
			Size = tilemapSize,
			CellSize = tilemapCellSize
		};
	}

	void Awake()
	{
		CacheGridData();
		InitGrid();
	}

	void CacheGridData()
	{
		// Edits to the tilemap don't refresh the bounds automatically
		tilemap.CompressBounds();

		tilemapPos = (Vector2)transform.position + new Vector2(tilemap.cellBounds.position.x, tilemap.cellBounds.position.y);
		tilemapSize = new Vector2(tilemap.cellBounds.size.x, tilemap.cellBounds.size.y);
		tilemapCellSize = GetComponent<Grid>().cellSize;
	}

	void InitGrid()
	{
		grid = new GridCell[(int)tilemapSize.x, (int)tilemapSize.y];

		for (int y = 0; y < grid.GetLength(1); y++)
		{
			for (int x = 0; x < grid.GetLength(0); x++)
			{
				Vector2Int gridPos = new Vector2Int(x, y);
				Vector2 worldPos = tilemapPos + gridPos + (tilemapCellSize / 2);
				bool isBlocked = IsTileBlocked(gridPos);

				GridCell cell = new GridCell(gridPos, worldPos, isBlocked);
				grid[x, y] = cell;
			}
		}
	}

	bool IsTileBlocked(Vector2 gridPos)
	{
		Vector2 halfCellSize = tilemapCellSize / 2;
		Vector2 cellCenterPos = tilemapPos + gridPos + halfCellSize;
		int obstacleLayers = LayerMask.GetMask(GameConstants.CollisionLayers.Obstacle);

		// Check if the center of a specific tile is walkable
		Collider2D[] obstacleColliders = Physics2D.OverlapBoxAll(cellCenterPos, halfCellSize, 0, obstacleLayers);

		return obstacleColliders.Length > 0;
	}
}
