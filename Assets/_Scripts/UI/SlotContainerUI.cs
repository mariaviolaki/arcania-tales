using System;
using System.Collections.Generic;
using UnityEngine;

public class SlotContainerUI : CanvasUI
{
	[SerializeField] InventorySettingsSO inventorySettings;
	[SerializeField] List<InventorySlot> slots;

	public Action<int, Vector2> OnSelectFullSlot;
	public Action<int, Vector2> OnSelectEmptySlot;

	override protected void Awake()
	{
		base.Awake();

		// Subscribe to click events for items in each slot
		foreach (InventorySlot slot in slots)
		{
			slot.OnSelectSlot += AccessSlot;
		}
	}

	override protected void OnDestroy()
	{
		base.OnDestroy();

		// Subscribe to click events for items in each slot
		foreach (InventorySlot slot in slots)
		{
			slot.OnSelectSlot -= AccessSlot;
		}
	}

	public void FillSlot(InventoryItem inventoryItem, int containerSlot)
	{
		if (inventoryItem.Quantity <= 0) return;

		slots[containerSlot].Fill(inventoryItem.Item, inventoryItem.Quantity);
	}

	public void EmptySlot(int containerSlot)
	{
		slots[containerSlot].Empty();
	}

	virtual protected void SelectFullSlot(ItemSO item, int quantity, int slot, Vector2 position)
	{
		if (this.GetType() != typeof(StorageChest))
		{
			OnSelectFullSlot?.Invoke(GetInventoryManagerSlot(slot), position);
		}
	}

	virtual protected void SelectEmptySlot(int slot, Vector2 position)
	{
		if (this.GetType() != typeof(StorageChest))
		{
			OnSelectEmptySlot?.Invoke(GetInventoryManagerSlot(slot), position);
		}
	}

	protected int GetInventoryManagerSlot(int slot)
	{
		if (GetType() == typeof(ToolbarUI)) return slot;

		return slot + inventorySettings.ToolSlots;
	}

	void AccessSlot(ItemSO item, int quantity, int slotIndex, Vector2 position)
	{
		if (item != null && quantity > 0)
		{
			SelectFullSlot(item, quantity, slotIndex, position);
		}
		else
		{
			SelectEmptySlot(slotIndex, position);
		}
	}
}
