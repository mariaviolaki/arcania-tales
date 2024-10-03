using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneMap", menuName = "Scriptable Objects/Scene Map")]
public class SceneMapSO : ScriptableObject
{
	[SerializeField] GameplaySettingsSO settings;
	[SerializeField] List<SceneSO> gameScenes;

	Dictionary<GameEnums.Scene, List<GamePosition>> map = new Dictionary<GameEnums.Scene, List<GamePosition>>();

	void OnEnable()
	{
		foreach (SceneSO scene in gameScenes)
		{
			map.Add(scene.Scene, scene.AdjacentScenes);
		}		
	}

	// The list of scenes to be crossed by a character to get to a certain scene
	public List<GamePosition> GetSceneRoute(GamePosition startPos, GamePosition endPos)
	{
		// Don't search routes for invalid scenes
		if (startPos == null || endPos == null) return null;
		if (startPos.Scene == GameEnums.Scene.None || endPos.Scene == GameEnums.Scene.None) return null;

		// This is a route within the same scene
		if (startPos.Scene == endPos.Scene) return new List<GamePosition>() { startPos, endPos };

		// This route spans across multiple scenes
		List<GamePosition> route = new List<GamePosition>();
		return SearchRoute(startPos, endPos, route);
	}

	List<GamePosition> SearchRoute(GamePosition current, GamePosition end, List<GamePosition> route)
	{
		if (route.Count > settings.MaxSceneRoute) return null;

		if (current.Scene == end.Scene)
		{
			// Come from either the starting position or the previous scene
			route.Add(GetExitToPreviousScene(route, end));
			// Always end at a random position inside the last scene
			route.Add(end);
			return route;
		}

		List<GamePosition> shortestRoute = null;
		foreach (GamePosition sceneEntry in map[current.Scene])
		{
			// Don't process circular routes
			if (route.FirstOrDefault(scenePos => scenePos.Scene == sceneEntry.Scene) != null) continue;

			List<GamePosition> newRoute = new List<GamePosition>(route);

			// Determine the starting position in this new scene
			newRoute.Add(GetExitToPreviousScene(route, current));
			// Head to the exit to the next scene
			newRoute.Add(new GamePosition(current.Scene, sceneEntry.Pos));

			// The route from the adjacent scene must lead to the target
			// Save it if it's the shortest one
			newRoute = SearchRoute(sceneEntry, end, newRoute);
			if (newRoute != null && (shortestRoute == null || newRoute.Count < shortestRoute.Count))
			{
				shortestRoute = newRoute;
			}
		}

		return shortestRoute;
	}

	GamePosition GetExitToPreviousScene(List<GamePosition> route, GamePosition currentPos)
	{
		GamePosition startPos = currentPos;
		if (route.Count != 0)
		{
			// Come from the exit to the previous scene
			GameEnums.Scene previousScene = route[route.Count - 1].Scene;
			GamePosition previousSceneExit = map[currentPos.Scene].FirstOrDefault(pos => pos.Scene == previousScene);
			startPos = new GamePosition(currentPos.Scene, previousSceneExit.Pos);
		}

		return startPos;
	}
}
