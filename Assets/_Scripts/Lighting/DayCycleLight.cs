using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycleLight : MonoBehaviour
{
	[SerializeField] GameObject lightContainer;
	[SerializeField] GameTime switchOnTime;
	[SerializeField] GameTime switchOffTime;

	DateManager dateManager;
	bool isSwitchedOn;

	void Start()
    {
		dateManager = FindObjectOfType<DateManager>();
		if (dateManager == null) return;

		isSwitchedOn = lightContainer.activeSelf;
		CheckDayCycleLight();

		dateManager.OnHourPassed += CheckDayCycleLight;
	}

	void OnDestroy()
	{
		if (dateManager == null) return;

		dateManager.OnHourPassed -= CheckDayCycleLight;
	}

	void CheckDayCycleLight()
	{
		GameTime gameTime = dateManager.GetTime();
		if ((gameTime >= switchOnTime || gameTime < switchOffTime) && !isSwitchedOn)
		{
			// Switch on during nighttime
			ToggleFlashlight(true);
		}
		else if (gameTime >= switchOffTime && gameTime < switchOnTime && isSwitchedOn)
		{
			// Switch off during daytime
			ToggleFlashlight(false);
		}
	}

	void ToggleFlashlight(bool isSwitchedOn)
	{
		// Keep track of the current state to avoid unnecessary calls to unity
		this.isSwitchedOn = isSwitchedOn;
		lightContainer.SetActive(isSwitchedOn);
	}
}
