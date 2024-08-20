using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DateUI : MonoBehaviour
{
	[SerializeField] TMP_Text dateText;
	[SerializeField] TMP_Text weekText;
	[SerializeField] TMP_Text timeText;

	DateManager dateManager;

	void Start()
    {
		dateManager = FindObjectOfType<DateManager>();
		if (dateManager == null) return;

		dateManager.OnTenMinutesPassed += UpdateTime;
		dateManager.OnHourPassed += UpdateDate;

		UpdateTime();
		UpdateDate();
	}

	void UpdateTime()
	{
		if (dateManager == null) return;

		timeText.text = $"{dateManager.GetDayHours():00}:{dateManager.GetMinutes():00}";
	}

	void UpdateDate()
	{
		if (dateManager == null) return;

		int seasonNum = dateManager.GetSeason() + 1;
		int seasonDayNum = dateManager.GetSeasonDay() + 1;
		int weekDayNum = dateManager.GetWeekDay() + 1;

		string seasonName = Enum.GetName(typeof(GameEnums.Season), seasonNum);
		string weekDayName = Enum.GetName(typeof(GameEnums.WeekDay), weekDayNum);

		dateText.text = $"{seasonName} {seasonDayNum}";
		weekText.text = weekDayName;
	}
}
