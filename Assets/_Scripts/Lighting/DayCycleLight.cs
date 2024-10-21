using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCycleLight : MonoBehaviour
{
	[SerializeField] protected GameObject lightContainer;
	[SerializeField] GameTime switchOnTime;
	[SerializeField] GameTime switchOffTime;

	protected DateManager dateManager;
	protected  bool isSwitchedOn;

	virtual protected void Start()
    {
		dateManager = FindObjectOfType<DateManager>();
		if (dateManager == null) return;

		isSwitchedOn = lightContainer.activeSelf;
		CheckHourlyLight();

		dateManager.OnHourPassed += CheckHourlyLight;
	}

	virtual protected void OnDestroy()
	{
		if (dateManager == null) return;

		dateManager.OnHourPassed -= CheckHourlyLight;
	}

	protected void CheckHourlyLight()
	{
		GameTime gameTime = dateManager.GetTime();
		if ((gameTime >= switchOnTime || gameTime < switchOffTime) && !isSwitchedOn)
		{
			// Switch on during nighttime
			ToggleLight(true);
		}
		else if (gameTime >= switchOffTime && gameTime < switchOnTime && isSwitchedOn)
		{
			// Switch off during daytime
			ToggleLight(false);
		}
	}

	protected void ToggleLight(bool isSwitchedOn)
	{
		// Keep track of the current state to avoid unnecessary calls to unity
		this.isSwitchedOn = isSwitchedOn;
		lightContainer.SetActive(isSwitchedOn);
	}
}
