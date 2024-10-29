using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class NpcMovement : CharacterMovement
{
	[Header("Character Params")]
	[SerializeField] NpcSO npcInfo;
	[SerializeField] NpcScheduleSO npcSchedule;

	[Header("Gameplay Params")]
	[SerializeField] GameplaySettingsSO gameplaySettings;
	[SerializeField] SceneMapSO sceneMap;
	[SerializeField] GameSceneManager sceneManager;
	[SerializeField] DateManager dateManager;
	[SerializeField] Pathfinder pathfinder;

	// Used to update the transform when in the same scene as the player
	GamePosition currentPos;
	// The regular facing direction defined only by the route schedule
	Vector2 scheduleFacingDirection;

	CapsuleCollider2D mainCollider;
	bool isInSameScene;
	bool isMoving;
	bool isSchedulePaused;

	public NpcSO NpcInfo { get { return npcInfo; } }

	void Awake()
	{
		mainCollider = GetComponent<CapsuleCollider2D>();
		currentPos = new GamePosition();
		scheduleFacingDirection = Vector2.down;
		isMoving = false;
		isSchedulePaused = false;

		UpdateWorldPosition(npcInfo.StartPosition.Scene, npcInfo.StartPosition.Pos, scheduleFacingDirection);
	}

	void Start()
	{
		isInSameScene = currentPos.Scene == sceneManager.CurrentScene;

		sceneManager.OnEndChangeScene += (Vector2 playerPos) => UpdateNpcVisibility();
		dateManager.OnTenMinutesPassed += CheckSchedule;

		OnChangeCharacterDirection?.Invoke(scheduleFacingDirection);
	}

	void OnDestroy()
	{
		sceneManager.OnEndChangeScene -= (Vector2 playerPos) => UpdateNpcVisibility();
		dateManager.OnTenMinutesPassed -= CheckSchedule;
	}

	public void SetSchedulePaused(bool isPaused)
	{
		SetSchedulePaused(isPaused, scheduleFacingDirection);
	}

	public void SetSchedulePaused(bool isPaused, Vector2 facingDirection)
	{
		isSchedulePaused = isPaused;
		OnChangeCharacterDirection?.Invoke(facingDirection);

		if (isPaused)
		{
			OnMoveCharacter?.Invoke(Vector2.zero);
		}
	}

	void CheckSchedule()
	{
		if (isMoving || npcSchedule == null) return;

		GameTime gameTime = dateManager.GetTime();
		GameEnums.Season season = (GameEnums.Season)(dateManager.GetSeason() + 1);
		GameEnums.WeekDay weekDay = (GameEnums.WeekDay)(dateManager.GetWeekDay() + 1);

		List<NpcRoutePoint> routePoints = npcSchedule.GetSchedule(season, weekDay);
		if (routePoints == null) return;

		foreach (NpcRoutePoint routePoint in routePoints)
		{
			if (routePoint.DepartureTime < gameTime) continue;
			else if (routePoint.DepartureTime > gameTime) break;

			List<GamePosition> sceneRoute = sceneMap.GetSceneRoute(currentPos, routePoint.TargetPosition);
			if (sceneRoute != null && sceneRoute.Count > 1)
			{
				StartCoroutine(FollowSchedule(sceneRoute, routePoint.FacingDirection));
			}

			break;
		}
	}

	// Move across different scenes in the game
	IEnumerator FollowSchedule(List<GamePosition> scenePositions, Vector2 facingDirection)
	{
		if (scenePositions == null || scenePositions.Count <= 1) yield return null;

		isMoving = true;

		for (int i = 0; i < scenePositions.Count; i++)
		{
			// If this is a starting point, calculate the path from the next one
			bool isNewScene = i == 0 || scenePositions[i].Scene != scenePositions[i - 1].Scene;
			if (isNewScene) continue;

			// Don't calculate paths in invalid scenes
			GameEnums.Scene pathScene = scenePositions[i].Scene;
			if (pathScene == GameEnums.Scene.None) break;

			// Attempt to follow a valid path between the two points in the same scene
			List<Vector2> routePositions = pathfinder.GetPath(pathScene, scenePositions[i - 1].Pos, scenePositions[i].Pos);
			if (routePositions == null || routePositions.Count == 0) break;

			yield return StartCoroutine(FollowRoute(pathScene, routePositions));
		}

		// After arriving at the target location, turn to look in the given direction
		scheduleFacingDirection = facingDirection;
		OnChangeCharacterDirection?.Invoke(facingDirection);

		isMoving = false;
		yield return null;
	}

	// Move between two points in the same scene
	IEnumerator FollowRoute(GameEnums.Scene routeScene, List<Vector2> routePositions)
	{
		foreach (Vector2 routePos in routePositions)
		{
			Vector2 startPos = currentPos.Pos;

			float distancePercentage = 0f;
			while (distancePercentage < 1f)
			{
				distancePercentage += Time.deltaTime * gameplaySettings.CharacterWalkSpeed;

				Vector2 newPos = Vector2.Lerp(startPos, routePos, distancePercentage);
				Vector2 direction = (routePos - startPos).normalized;
				UpdateWorldPosition(routeScene, newPos, direction);

				yield return new WaitUntil(() => !isSchedulePaused);
			}
		}

		OnMoveCharacter?.Invoke(Vector2.zero);
	}

	void UpdateWorldPosition(GameEnums.Scene newScene, Vector2 newPos, Vector2 direction)
	{
		currentPos.Scene = newScene;
		currentPos.Pos = newPos;

		bool isVisible = UpdateNpcVisibility();
		if (isVisible)
		{
			OnMoveCharacter?.Invoke(direction);
		}
	}

	bool UpdateNpcVisibility()
	{
		if (isSchedulePaused) return false;

		// Only update the transform if the npc is in the same scene as the player
		bool isInNewScene = currentPos.Scene == sceneManager.CurrentScene;
		if (isInNewScene)
		{
			transform.position = currentPos.Pos;
		}

		// Don't enable/disable any NPC components if not necessary
		if (isInSameScene != isInNewScene)
		{
			isInSameScene = isInNewScene;

			// Only reveal the npc if they are present in the current scene
			SetVisible(isInNewScene);

			if (isInNewScene)
			{
				// Turn in the correct direction because the animator was disabled
				OnChangeCharacterDirection?.Invoke(scheduleFacingDirection);
			}
		}

		return isInNewScene;
	}

	void SetVisible(bool isVisible)
	{
		mainCollider.enabled = isVisible;
		foreach (Transform bodyPart in transform)
		{
			bodyPart.gameObject.SetActive(isVisible);
		}
	}
}
