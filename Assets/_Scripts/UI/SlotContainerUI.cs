using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class SlotContainerUI : CanvasUI
{
	[SerializeField] protected InventorySettingsSO inventorySettings;
	[SerializeField] protected InventoryManager inventoryManager;
	[SerializeField] protected Transform slotContainer;

	List<SlotUI> slots;

	public Action<int, Vector2> OnSelectFullSlot;
	public Action<int, Vector2> OnSelectEmptySlot;

	protected List<SlotUI> Slots { get { return slots; } }

	abstract protected int GetStartSlot();
	abstract protected int GetEndSlot();

	override protected void Awake()
	{
		base.Awake();

		slots = new List<SlotUI>();
	}

	override protected void Start()
	{
		base.Start();

		foreach (Transform slotTransform in slotContainer)
		{
			// Cache each slot in the container and subscribe to click events for items
			SlotUI slot = slotTransform.GetComponent<SlotUI>();
			slot.OnSelectSlot += AccessSlot;
			slots.Add(slot);
		}
	}

	override protected void OnDestroy()
	{
		base.OnDestroy();

		// Subscribe to click events for items in each slot
		foreach (SlotUI slot in slots)
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
		OnSelectFullSlot?.Invoke(GetStartSlot() + slot, position);
	}

	virtual protected void SelectEmptySlot(int slot, Vector2 position)
	{
		OnSelectEmptySlot?.Invoke(GetStartSlot() + slot, position);
	}	

	void RefreshSlots()
	{
		if (GetType() == typeof(StorageUI)) return;

		List<InventoryItem> inventoryItems = inventoryManager.Items;
		int inventoryManagerStartSlot = GetStartSlot();

		for (int childSlot = 0; childSlot < slots.Count; childSlot++)
		{
			InventoryItem inventoryItem = inventoryItems[inventoryManagerStartSlot + childSlot];
			if (inventoryItem == null)
			{
				slots[childSlot].Empty();
			}
			else
			{
				slots[childSlot].Fill(inventoryItem.Item, inventoryItem.Quantity);
			}
		}
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
