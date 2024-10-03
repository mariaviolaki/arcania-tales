using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class NpcWeekSchedule
{
	[SerializeField] List<NpcRoutePoint> monday;
	[SerializeField] List<NpcRoutePoint> tuesday;
	[SerializeField] List<NpcRoutePoint> wednesday;
	[SerializeField] List<NpcRoutePoint> thursday;
	[SerializeField] List<NpcRoutePoint> friday;
	[SerializeField] List<NpcRoutePoint> saturday;
	[SerializeField] List<NpcRoutePoint> sunday;

	public List<NpcRoutePoint> GetSchedule(GameEnums.WeekDay weekDay)
	{
		switch (weekDay)
		{
			case GameEnums.WeekDay.Monday: return monday;
			case GameEnums.WeekDay.Tuesday: return tuesday;
			case GameEnums.WeekDay.Wednesday: return wednesday;
			case GameEnums.WeekDay.Thursday: return thursday;
			case GameEnums.WeekDay.Friday: return friday;
			case GameEnums.WeekDay.Saturday: return saturday;
			case GameEnums.WeekDay.Sunday: return sunday;
			default: return null;
		}
	}
}

[System.Serializable]
public struct NpcRoutePoint
{
	[SerializeField] GameEnums.Direction facingDirection;
	[SerializeField] GamePosition targetPosition;
	[SerializeField] GameTime departureTime;

	public GamePosition TargetPosition { get { return targetPosition; } }
	public GameTime DepartureTime { get { return departureTime; } }
	public Vector2 FacingDirection
	{
		get { 
			switch (facingDirection)
			{
				case GameEnums.Direction.Up: return Vector2.up;
				case GameEnums.Direction.Left: return Vector2.left;
				case GameEnums.Direction.Right: return Vector2.right;
				default: return Vector2.down;
			}
		}
	}
}