using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameTime
{
	[SerializeField] int hours;
	[SerializeField] int minutes;

	public int Hours { get { return hours; } }
	public int Minutes { get { return minutes; } }

	public GameTime(int hours, int minutes)
	{
		this.hours = hours;
		this.minutes = minutes;
	}

	public override bool Equals(object obj)
	{
		return base.Equals(obj);
	}

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override string ToString()
	{
		return base.ToString();
	}

	public static bool operator ==(GameTime left, GameTime right)
	{
		return left.Hours == right.Hours && left.Minutes == right.Minutes;
	}

	public static bool operator !=(GameTime left, GameTime right)
	{
		return left.Hours != right.Hours && left.Minutes != right.Minutes;
	}

	public static bool operator <(GameTime left, GameTime right)
	{
		return (left.Hours < right.Hours) || (left.Hours == right.Hours && left.Minutes < right.Minutes);
	}

	public static bool operator >(GameTime left, GameTime right)
	{
		return (left.Hours > right.Hours) || (left.Hours == right.Hours && left.Minutes > right.Minutes);
	}

	public static bool operator <=(GameTime left, GameTime right)
	{
		return left < right || left == right;
	}

	public static bool operator >=(GameTime left, GameTime right)
	{
		return left > right || left == right;
	}
}
