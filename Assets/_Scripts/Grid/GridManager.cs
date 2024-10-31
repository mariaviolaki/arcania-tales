using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
	[SerializeField] GameSceneManager sceneManager;

	Dictionary<GameEnums.Scene, GridData> grids = new Dictionary<GameEnums.Scene, GridData>();

	void Awake()
	{
		sceneManager.OnLoadGameScenes += LoadGridData;

		StartCoroutine(sceneManager.LoadGameScenes());
	}

	void OnDestroy()
	{
		sceneManager.OnLoadGameScenes -= LoadGridData;
	}

	public GridCell GetGridCell(GameEnums.Scene scene, int x, int y, bool isPathEdge)
	{
		if (scene == GameEnums.Scene.None || grids[scene].Grid == null) return null;
		if (!IsValidPathCell(scene, x, y, isPathEdge)) return null;

		return grids[scene].Grid[x, y];
	}

	public Vector2 GetGridPos(GameEnums.Scene scene)
	{
		if (scene == GameEnums.Scene.None || grids[scene].Pos == null) return Vector2.negativeInfinity;

		return grids[scene].Pos;
	}

	public Vector2 GetGridCellSize(GameEnums.Scene scene)
	{
		if (scene == GameEnums.Scene.None || grids[scene].CellSize == null) return Vector2.negativeInfinity;

		return grids[scene].CellSize;
	}

	void LoadGridData()
	{
		foreach (TerrainGrid terrainGrid in FindObjectsOfType<TerrainGrid>())
		{
			grids[terrainGrid.Scene] = terrainGrid.GetGridData();
		}

		StartCoroutine(sceneManager.UnloadGameScenes());
	}

	bool IsValidPathCell(GameEnums.Scene scene, int x, int y, bool isPathEdge)
	{
		bool isValidX = x >= 0 && x < grids[scene].Grid.GetLength(0);
		bool isValidY = y >= 0 && y < grids[scene].Grid.GetLength(1);
		if (!isValidX || !isValidY) return false;

		bool isBlocked = grids[scene].Grid[x, y].IsBlocked;
		return !isBlocked || isPathEdge;
	}
}
