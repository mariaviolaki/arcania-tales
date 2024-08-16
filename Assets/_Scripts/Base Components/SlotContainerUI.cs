using System;
using System.Collections.Generic;
using UnityEngine;

public class SlotContainerUI : MonoBehaviour
{
	[SerializeField] GameSettingsSO gameSettings;
	[SerializeField] List<InventorySlot> slots;

	public Action<int, Vector2> OnSelectFullSlot;
	public Action<int, Vector2> OnSelectEmptySlot;

	protected void InitSlotListeners()
	{
		// Subscribe to click events for items in each slot
		foreach (InventorySlot slot in slots)
		{
			slot.OnSelectSlot += AccessSlot;
		}
	}

	public void FillSlot(InventoryItem inventoryItem)
	{
		if (inventoryItem.Quantity <= 0) return;

		int childSlot = GetChildSlot(inventoryItem.Slot);
		slots[childSlot].Fill(inventoryItem.Item, inventoryItem.Quantity);
	}

	public void EmptySlot(int slot)
	{
		int childSlot = GetChildSlot(slot);
		slots[childSlot].Empty();
	}

	void AccessSlot(CollectableSO collectable, int quantity, int slotIndex, Vector2 position)
	{
		if (collectable != null && quantity > 0)
		{
			SelectFullSlot(collectable, quantity, slotIndex, position);
		}
		else
		{
			SelectEmptySlot(slotIndex, position);
		}
	}

	void SelectFullSlot(CollectableSO collectable, int quantity, int slot, Vector2 position)
	{
		OnSelectFullSlot?.Invoke(GetInventoryManagerSlot(slot), position);
	}

	void SelectEmptySlot(int slot, Vector2 position)
	{
		OnSelectEmptySlot?.Invoke(GetInventoryManagerSlot(slot), position);
	}

	/*
		The total slots are not separated into groups in the Inventory Manager
		But visually, items are stored in different containers: Toolbar, Inventory, and Storage
		Because the Toolbar is always present, Inventory and Storage slots come after
	*/

	int GetChildSlot(int slot)
	{
		if (GetType() == typeof(ToolbarUI)) return slot;

		return slot - gameSettings.ToolSlots;
	}

	int GetInventoryManagerSlot(int slot)
	{
		if (GetType() == typeof(ToolbarUI)) return slot;

		return slot + gameSettings.ToolSlots;
	}
}
