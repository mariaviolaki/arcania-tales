using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFlashlight : DayCycleLight
{
	GameSceneManager sceneManager;

	override protected void Start()
	{
		dateManager = FindObjectOfType<DateManager>();
		sceneManager = FindObjectOfType<GameSceneManager>();
		if (dateManager == null || sceneManager == null) return;

		isSwitchedOn = lightContainer.activeSelf;
		CheckSceneLight();

		dateManager.OnHourPassed += CheckSceneLight;
		sceneManager.OnEndChangeScene += (Vector2 playerPos) => CheckSceneLight();
	}

	override protected void OnDestroy()
	{
		if (dateManager == null || sceneManager == null) return;

		dateManager.OnHourPassed -= CheckSceneLight;
		sceneManager.OnEndChangeScene -= (Vector2 playerPos) => CheckSceneLight();
	}

	void CheckSceneLight()
	{
		bool isOutdoorScene = GameUtils.IsOutdoorScene(sceneManager.CurrentScene);
		if (isOutdoorScene)
		{
			// Toggle the flashlight normally while outside
			CheckHourlyLight();
		}
		else if (isSwitchedOn)
		{
			// Disable the flashlight in indoor spaces
			ToggleLight(false);
		}	
	}
}
