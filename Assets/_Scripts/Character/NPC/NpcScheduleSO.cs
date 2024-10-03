using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NpcSchedule", menuName = "Scriptable Objects/Npc Schedule")]
public class NpcScheduleSO : ScriptableObject
{
	[SerializeField] NpcWeekSchedule springSchedule;
	[SerializeField] NpcWeekSchedule summerSchedule;
	[SerializeField] NpcWeekSchedule fallSchedule;
	[SerializeField] NpcWeekSchedule winterSchedule;

	public NpcWeekSchedule Spring { get { return springSchedule; } }
	public NpcWeekSchedule Summer { get { return summerSchedule; } }
	public NpcWeekSchedule Autumn { get { return fallSchedule; } }
	public NpcWeekSchedule Winter { get { return winterSchedule; } }

	public List<NpcRoutePoint> GetSchedule(GameEnums.Season season, GameEnums.WeekDay weekDay)
	{
		if (season == GameEnums.Season.Spring) return springSchedule.GetSchedule(weekDay);
		else if (season == GameEnums.Season.Summer) return summerSchedule.GetSchedule(weekDay);
		else if (season == GameEnums.Season.Fall) return fallSchedule.GetSchedule(weekDay);
		else if (season == GameEnums.Season.Winter) return winterSchedule.GetSchedule(weekDay);

		return null;
	}
}

