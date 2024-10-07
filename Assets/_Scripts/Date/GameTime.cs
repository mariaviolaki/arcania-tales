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

	public static GameTime operator +(GameTime left, GameTime right)
	{
		int minutesSum = Mathf.Min(left.minutes + right.minutes, 59);
		int minutesExtra = left.minutes + right.minutes - minutesSum;

		int hoursExtra = (int)Mathf.Ceil(minutesExtra / 59f);
		int hoursSum = left.hours + right.hours + hoursExtra;

		return new GameTime(hoursSum, minutesSum);
	}

	public static bool operator ==(GameTime left, GameTime right)
	{
		if (left is null) return right is null;
		else if (left is null || right is null) return false;

		return left.Hours == right.Hours && left.Minutes == right.Minutes;
	}

	public static bool operator !=(GameTime left, GameTime right)
	{
		if (left is null && right is null) return false;
		else if (left is null || right is null) return true;

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
}
