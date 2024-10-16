using System;
using System.Collections.Generic;
using UnityEngine;

public class SlotContainerUI : CanvasUI
{
	[SerializeField] InventorySettingsSO inventorySettings;
	[SerializeField] Transform slotContainer;

	List<InventorySlot> slots;

	public Action<int, Vector2> OnSelectFullSlot;
	public Action<int, Vector2> OnSelectEmptySlot;

	protected List<InventorySlot> Slots { get { return slots; } }

	override protected void Awake()
	{
		base.Awake();

		slots = new List<InventorySlot>();
	}

	override protected void Start()
	{
		base.Start();

		foreach (Transform slotTransform in slotContainer)
		{
			// Cache each slot in the container and subscribe to click events for items
			InventorySlot slot = slotTransform.GetComponent<InventorySlot>();
			slot.OnSelectSlot += AccessSlot;
			slots.Add(slot);
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
		if (GetType() == typeof(InventoryUI))
		{
			// The inventory slots are handled as the toolbar's continuation
			return slot + inventorySettings.ToolSlots;
		}

		return slot;
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
