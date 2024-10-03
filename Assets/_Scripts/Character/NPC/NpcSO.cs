using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Npc", menuName = "Scriptable Objects/Npc")]
public class NpcSO : ScriptableObject
{
	[SerializeField] string npcName;
	[SerializeField] GamePosition startPosition;
	[SerializeField] NpcScheduleSO routeSchedule;

	public string NpcName { get { return npcName; } }
	public GamePosition StartPosition { get { return startPosition; } }
	public NpcScheduleSO RouteSchedule { get { return routeSchedule; } }
}
