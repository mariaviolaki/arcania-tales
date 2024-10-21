using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEntry : MonoBehaviour, IInteractable
{
	[SerializeField] SceneMapSO sceneMap;
	[SerializeField] GameEnums.Scene nextScene;

	public GameEnums.Scene NextScene { get { return nextScene; } }

	public void Interact(Transform player)
	{
		PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
		if (playerMovement == null) return;

		playerMovement.ChangeScene(this);
	}

	public Vector2 GetEntryPoint(GameEnums.Scene currentScene)
	{
		return sceneMap.GetEntryPos(currentScene, nextScene);
	}
}
