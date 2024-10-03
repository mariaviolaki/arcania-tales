using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DateManager : MonoBehaviour
{
	[SerializeField] DateSettingsSO dateSettings;

	int minutes;
	int hours;
	float timeElapsed;

	public Action OnMinutePassed;
	public Action OnTenMinutesPassed;
	public Action OnHourPassed;
	public Action OnYearPassed;

	public int GetMinutes() { return minutes; }
	public int GetTotalHours() { return hours; }
	public int GetTotalDays() { return hours / dateSettings.HoursPerDay; }

	public int GetDayHours() { return hours % dateSettings.HoursPerDay; }
	public int GetWeekDay() { return GetTotalDays() % dateSettings.DaysPerWeek; }
	public int GetSeasonDay() { return GetTotalDays() % dateSettings.DaysPerSeason; }
	public int GetSeason() { return GetYearDay() / dateSettings.DaysPerSeason; }
	public int GetYear()
	{
		int seasonCount = Enum.GetNames(typeof(GameEnums.Season)).Length;
		int hoursPerYear = dateSettings.HoursPerDay * dateSettings.DaysPerSeason * seasonCount;

		return hours / hoursPerYear;
	}
	public int GetYearDay()
	{
		int seasonCount = Enum.GetNames(typeof(GameEnums.Season)).Length;
		int hoursPerYear = dateSettings.HoursPerDay * dateSettings.DaysPerSeason * seasonCount;
		int yearHours = hours % hoursPerYear;
		
		return yearHours / dateSettings.HoursPerDay;
	}

	public GameTime GetTime() { return new GameTime(GetDayHours(), GetMinutes()); }

	void Awake()
	{
		InitDateAndTime();
	}

	void Update()
	{
		UpdateTimer();
	}

	void InitDateAndTime()
	{
		int hoursPerSeason = dateSettings.DaysPerSeason * dateSettings.HoursPerDay;
		int seasonCount = Enum.GetNames(typeof(GameEnums.Season)).Length;

		int yearHours = seasonCount * hoursPerSeason * (dateSettings.StartYear - 1);
		int seasonHours = hoursPerSeason * (dateSettings.StartSeason - 1);
		int dayHours = dateSettings.HoursPerDay * (dateSettings.StartDay - 1);

		hours = yearHours + seasonHours + dayHours + dateSettings.StartHour;
		minutes = dateSettings.StartMinute;
	}

	void UpdateTimer()
	{
		timeElapsed += Time.deltaTime;
		if (timeElapsed >= dateSettings.GameSecondsPerSecond / dateSettings.SecondsPerMinute)
		{
			UpdateMinutes();
			timeElapsed = 0f;
		}
	}

	void UpdateMinutes()
	{
		minutes++;
		if (minutes == dateSettings.MinutesPerHour)
		{
			minutes = 0;
			UpdateHours();
		}

		OnMinutePassed?.Invoke();

		if (minutes % 10 == 0)
		{
			OnTenMinutesPassed?.Invoke();
		}
	}

	void UpdateHours()
	{
		hours++;
		OnHourPassed?.Invoke();
	}
}
