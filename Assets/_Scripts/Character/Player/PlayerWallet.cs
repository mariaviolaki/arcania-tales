using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallet : MonoBehaviour
{
	[SerializeField] GameplaySettingsSO settings;

	int currentGold;

	public Action OnGoldChange;

	public int Gold { get { return currentGold; } }

	void Awake()
	{
		currentGold = settings.StartGold;
	}

	public bool AddGold(int amount)
	{
		if (amount < 0) return false;

		currentGold += amount;
		OnGoldChange?.Invoke();

		return true;
	}

	public bool RemoveGold(int amount)
	{
		if (amount < 0 || currentGold - amount < 0) return false;

		currentGold -= amount;
		OnGoldChange?.Invoke();

		return true;
	}
}
