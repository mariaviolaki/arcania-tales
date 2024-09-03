using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCSchedule", menuName = "Scriptable Objects/NPC Schedule")]
public class NPCScheduleSO : ScriptableObject
{
	[SerializeField] NPCWeekSchedule[] springSchedule;
	[SerializeField] NPCWeekSchedule[] summerSchedule;
	[SerializeField] NPCWeekSchedule[] autumnSchedule;
	[SerializeField] NPCWeekSchedule[] winterSchedule;

	public NPCWeekSchedule[] Spring { get { return springSchedule; } }
	public NPCWeekSchedule[] Summer { get { return summerSchedule; } }
	public NPCWeekSchedule[] Autumn { get { return autumnSchedule; } }
	public NPCWeekSchedule[] Winter { get { return winterSchedule; } }
}

