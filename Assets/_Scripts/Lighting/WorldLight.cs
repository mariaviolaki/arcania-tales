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
	[SerializeField] GameSceneManager sceneManager;
	[SerializeField] DateSettingsSO dateSettings;

	Light2D worldLight;

	void Awake()
	{
		worldLight = GetComponent<Light2D>();
	}

	void Start()
    {
		dateManager.OnHourPassed += UpdateLightColor;
		sceneManager.OnEndChangeScene += (Vector2 entryPos) => UpdateLightColor();

		UpdateLightColor();
    }

	void OnDestroy()
	{
		dateManager.OnHourPassed -= UpdateLightColor;
		sceneManager.OnEndChangeScene -= (Vector2 entryPos) => UpdateLightColor();
	}

	void UpdateLightColor()
	{
		if (!GameUtils.IsOutdoorScene(sceneManager.CurrentScene))
		{
			worldLight.color = Color.white;
			return;
		}

		// Find the amount of minutes that have passed in this day
		GameTime gameTime = dateManager.GetTime();
		int dayMinutes = (gameTime.Hours * dateSettings.MinutesPerHour) + gameTime.Minutes;

		// Find how much of the day has passed and apply the color accordingly
		int totalMinutes = dateSettings.HoursPerDay * dateSettings.MinutesPerHour;
		float dayPercentage = (float)dayMinutes / totalMinutes;

		worldLight.color = lightColors.Evaluate(dayPercentage);
	}
}
