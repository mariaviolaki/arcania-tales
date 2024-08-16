using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
	[SerializeField] GameSettingsSO gameSettings;

	public Action<InventoryItem, InventoryItem> OnUpdateInventoryUI;
	List<InventoryItem> items;

	void Awake()
	{
		int maxCapacity = gameSettings.ToolSlots + gameSettings.InventorySlots;
		items = new List<InventoryItem>(Enumerable.Repeat((InventoryItem)null, maxCapacity));
	}

	// Automatically add a collectable to the inventory without choosing its slot
	public bool AddCollectable(CollectableSO collectable, int quantity = 1)
	{
		int firstAvailableSlot = int.MaxValue;

		for (int i = 0; i < gameSettings.ToolSlots + gameSettings.InventorySlots; i++)
		{
			InventoryItem inventoryItem = items[i];

			// Save the first free slot in case this item cannot be stacked
			if (inventoryItem == null && i < firstAvailableSlot)
			{
				firstAvailableSlot = i;
			}
			// Attempt to stack the item in the first available slot
			else if (inventoryItem != null)
			{
				bool isSameItemSlot = inventoryItem.Item.Name == collectable.Name;
				bool isFreeSpaceSlot = inventoryItem.Quantity < gameSettings.MaxSlotQuantity;

				if (isSameItemSlot && isFreeSpaceSlot)
				{
					return StackCollectable(new InventoryItem(collectable, quantity, -1), i);
				}
			}
		}

		return AddNewCollectable(new InventoryItem(collectable, quantity, -1), firstAvailableSlot);
	}

	// Manually rearrange the inventory by forcing an item into a specific slot
	public bool AddItemToSlot(InventoryItem inventoryItem, int slot)
	{
		int maxCapacity = gameSettings.ToolSlots + gameSettings.InventorySlots;
		if (inventoryItem == null || inventoryItem.Quantity <= 0 || slot < 0 || slot >= maxCapacity) return false;

		if (items[slot] == null)
		{
			return AddNewCollectable(inventoryItem, slot);
		}
		else if (items[slot].Item.Name == inventoryItem.Item.Name)
		{
			return StackCollectable(inventoryItem, slot);
		}
		else
		{
			return SwapCollectables(inventoryItem, slot);
		}
	}

	public bool RemoveItemFromSlot(int slot)
	{
		if (slot < 0 || items[slot] == null) return false;

		InventoryItem remainingItem = new InventoryItem(items[slot].Item, items[slot].Quantity, slot);
		items[slot] = null;

		OnUpdateInventoryUI?.Invoke(null, remainingItem);
		return true;
	}

	bool AddNewCollectable(InventoryItem inventoryItem, int slot)
	{
		// Ensure that the new item will not exceed the max slot quantity
		int slotQuantity = Mathf.Min(inventoryItem.Quantity, gameSettings.MaxSlotQuantity);
		int remainingQuantity = inventoryItem.Quantity - slotQuantity;

		InventoryItem newItem = new InventoryItem(inventoryItem.Item, slotQuantity, slot);
		InventoryItem remainingItem = new InventoryItem(inventoryItem.Item, remainingQuantity, inventoryItem.Slot);
		items[slot] = newItem;

		OnUpdateInventoryUI?.Invoke(newItem, remainingItem);
		return true;
	}

	bool StackCollectable(InventoryItem inventoryItem, int slot)
	{
		// Check if this slot is already full
		InventoryItem existingItem = items[slot];
		int slotQuantity = Mathf.Min(inventoryItem.Quantity, gameSettings.MaxSlotQuantity - existingItem.Quantity);
		if (slotQuantity <= 0) return false;

		// TODO test if inventoryItem is permanently changed as expected
		existingItem.Quantity += slotQuantity;
		int remainingQuantity = inventoryItem.Quantity - slotQuantity;
		InventoryItem remainingItem = new InventoryItem(inventoryItem.Item, remainingQuantity, inventoryItem.Slot);

		OnUpdateInventoryUI?.Invoke(existingItem, remainingItem);
		return true;
	}

	bool SwapCollectables(InventoryItem inventoryItem, int slot)
	{
		if (inventoryItem.Quantity > gameSettings.MaxSlotQuantity) return false;

		InventoryItem existingItem = items[slot];
		existingItem.Slot = inventoryItem.Slot;

		// TODO test if the two separate items show as expected
		InventoryItem newItem = new InventoryItem(inventoryItem.Item, inventoryItem.Quantity, slot);
		items[slot] = newItem;

		OnUpdateInventoryUI?.Invoke(newItem, existingItem);
		return true;
	}
}
