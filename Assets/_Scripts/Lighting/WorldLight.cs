using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class WorldLight : MonoBehaviour
{
	[Tooltip("The light color applied over the world at any time of the day")]
	[SerializeField] Gradient lightColors;
	[SerializeField] DateManager dateManager;
	[SerializeField] DateSettingsSO dateSettings;

	Light2D worldLight;

	void Awake()
	{
		worldLight = GetComponent<Light2D>();
	}

	void Start()
    {
		dateManager.OnHourPassed += UpdateLightColor;

		UpdateLightColor();
    }

	void OnDestroy()
	{
		dateManager.OnHourPassed -= UpdateLightColor;
	}

	void UpdateLightColor()
	{
		// Find the amount of minutes that have passed in this day
		GameTime gameTime = dateManager.GetTime();
		int dayMinutes = (gameTime.Hours * dateSettings.MinutesPerHour) + gameTime.Minutes;

		// Find how much of the day has passed and apply the color accordingly
		int totalMinutes = dateSettings.HoursPerDay * dateSettings.MinutesPerHour;
		float dayPercentage = (float)dayMinutes / totalMinutes;

		worldLight.color = lightColors.Evaluate(dayPercentage);
	}
}
