using UnityEngine;

[CreateAssetMenu(fileName = "DateSettings", menuName = "Scriptable Objects/Date Settings")]
public class DateSettingsSO : ScriptableObject
{
	[Header("Date Calculations")]
	[SerializeField] float gameSecondsPerSecond;
	[SerializeField] int secondsPerMinute;
	[SerializeField] int minutesPerHour;
	[SerializeField] int hoursPerDay;
	[SerializeField] int daysPerWeek;
	[SerializeField] int daysPerSeason;

	[Header("Game Start")]
	[SerializeField] int startYear;
	[SerializeField] int startSeason;
	[SerializeField] int startDay;
	[SerializeField] int startHour;
	[SerializeField] int startMinute;

	public float GameSecondsPerSecond { get { return gameSecondsPerSecond; } }
	public int SecondsPerMinute { get { return secondsPerMinute; } }
	public int MinutesPerHour { get { return minutesPerHour; } }
	public int HoursPerDay { get { return hoursPerDay; } }
	public int DaysPerWeek { get { return daysPerWeek; } }
	public int DaysPerSeason { get { return daysPerSeason; } }

	public int StartYear { get { return startYear; } }
	public int StartSeason { get { return startSeason; } }
	public int StartDay { get { return startDay; } }
	public int StartHour { get { return startHour; } }
	public int StartMinute { get { return startMinute; } }
}
