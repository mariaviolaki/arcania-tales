using System;
using System.Collections.Generic;
using UnityEngine;

public class StorageUI : SlotContainerUI
{
	StorageChest currentStorage;

	public Action<int, Vector2, StorageChest> OnSelectFullStorageSlot;
	public Action<int, Vector2, StorageChest> OnSelectEmptyStorageSlot;

	public StorageChest CurrentStorage { get { return currentStorage; } set { currentStorage = value; } }
	
	public void SetCurrentStorage(StorageChest storageChest)
	{
		if (storageChest == null)
		{
			currentStorage.ToggleStorage(false);
			currentStorage = storageChest;
		}
		else
		{
			currentStorage = storageChest;
			currentStorage.ToggleStorage(true);
			LoadItems();
		}
	}

	void LoadItems()
	{
		List<InventoryItem> items = inventoryManager.GetStorageItems(currentStorage);

		int index = 0;
		foreach (SlotUI slot in Slots)
		{
			InventoryItem inventoryItem = items[index++];

			// Fill this slot in the UI with the correct data for this specific storage
			if (inventoryItem == null)
			{
				slot.Empty();
			}
			else
			{
				slot.Fill(inventoryItem.Item, inventoryItem.Quantity);
			}
		}
	}

	override protected void SelectFullSlot(ItemSO item, int quantity, int slot, Vector2 position)
	{
		OnSelectFullStorageSlot?.Invoke(GetStartSlot() + slot, position, currentStorage);
	}

	override protected void SelectEmptySlot(int slot, Vector2 position)
	{
		OnSelectEmptyStorageSlot?.Invoke(GetStartSlot() + slot, position, currentStorage);
	}

	protected override int GetStartSlot()
	{
		return 0;
	}

	protected override int GetEndSlot()
	{
		return inventorySettings.StorageSlots - 1;
	}
}
