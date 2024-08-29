using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Pathfinder))]
public class AIMovement : MonoBehaviour
{
	[SerializeField] GameplaySettingsSO gameplaySettings;
	[SerializeField] Vector2 targetPosition; // TODO: Get this from SO with NPC routes

	Pathfinder pathfinder;
	List<Vector2> routePositions;

	public Action<Vector2> OnMoveAI;

	void Start()
	{
		pathfinder = GetComponent<Pathfinder>();
		routePositions = pathfinder.GetPath(transform.position, targetPosition);

		if (routePositions.Count > 0)
		{
			StartCoroutine(FollowRoute());
		}
	}

	IEnumerator FollowRoute()
	{
		foreach (Vector2 routePos in routePositions)
		{
			Vector2 startPos = transform.position;

			float distancePercentage = 0f;
			while (distancePercentage < 1f)
			{
				distancePercentage += Time.deltaTime * gameplaySettings.NpcWalkSpeed;
				transform.position = Vector2.Lerp(startPos, routePos, distancePercentage);

				Vector2 direction = (routePos - startPos).normalized;
				OnMoveAI?.Invoke(direction);

				yield return null;
			}
		}

		OnMoveAI?.Invoke(Vector2.zero);
	}
}
