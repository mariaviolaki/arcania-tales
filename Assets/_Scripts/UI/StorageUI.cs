using System;
using UnityEngine;

public class StorageUI : SlotContainerUI
{
	StorageChest currentStorage;

	public Action<int, Vector2, StorageChest> OnSelectFullStorageSlot;
	public Action<int, Vector2, StorageChest> OnSelectEmptyStorageSlot;

	public StorageChest CurrentStorage { get { return currentStorage; } set { currentStorage = value; } }

	override protected void SelectFullSlot(ItemSO item, int quantity, int slot, Vector2 position)
	{
		if (this.GetType() != typeof(StorageChest))
		{
			OnSelectFullStorageSlot?.Invoke(GetInventoryManagerSlot(slot), position, currentStorage);
		}
	}

	override protected void SelectEmptySlot(int slot, Vector2 position)
	{
		if (this.GetType() != typeof(StorageChest))
		{
			OnSelectEmptyStorageSlot?.Invoke(GetInventoryManagerSlot(slot), position, currentStorage);
		}
	}
}
