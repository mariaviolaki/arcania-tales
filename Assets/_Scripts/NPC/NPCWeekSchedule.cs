using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct NPCWeekSchedule
{
	public List<NPCRoutePoint> Monday;
	public List<NPCRoutePoint> Tuesday;
	public List<NPCRoutePoint> Wednesday;
	public List<NPCRoutePoint> Thursday;
	public List<NPCRoutePoint> Friday;
	public List<NPCRoutePoint> Saturday;
	public List<NPCRoutePoint> Sunday;
}
