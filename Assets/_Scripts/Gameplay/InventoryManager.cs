using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
	[SerializeField] InventorySettingsSO inventorySettings;

	public Action<InventoryItem, InventoryItem> OnUpdateInventoryUI;
	List<InventoryItem> items;

	void Awake()
	{
		int maxCapacity = inventorySettings.ToolSlots + inventorySettings.InventorySlots;
		items = new List<InventoryItem>(Enumerable.Repeat((InventoryItem)null, maxCapacity));
	}

	// Automatically add an item to the inventory without choosing its slot
	public bool AddItem(ItemSO item, int quantity = 1)
	{
		int firstAvailableSlot = int.MaxValue;

		for (int i = 0; i < inventorySettings.ToolSlots + inventorySettings.InventorySlots; i++)
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
				bool isSameItemSlot = inventoryItem.Item.Name == item.Name;
				bool isFreeSpaceSlot = inventoryItem.Quantity < inventorySettings.MaxSlotQuantity;

				if (isSameItemSlot && isFreeSpaceSlot)
				{
					return StackItem(new InventoryItem(item, quantity, -1), i);
				}
			}
		}

		return AddNewItem(new InventoryItem(item, quantity, -1), firstAvailableSlot);
	}

	// Manually rearrange the inventory by forcing an item into a specific slot
	public bool AddItemToSlot(InventoryItem inventoryItem, int slot)
	{
		int maxCapacity = inventorySettings.ToolSlots + inventorySettings.InventorySlots;
		if (inventoryItem == null || inventoryItem.Quantity <= 0 || slot < 0 || slot >= maxCapacity) return false;

		if (items[slot] == null)
		{
			return AddNewItem(inventoryItem, slot);
		}
		else if (items[slot].Item.Name == inventoryItem.Item.Name)
		{
			return StackItem(inventoryItem, slot);
		}
		else
		{
			return SwapItems(inventoryItem, slot);
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

	bool AddNewItem(InventoryItem inventoryItem, int slot)
	{
		// Ensure that the new item will not exceed the max slot quantity
		int slotQuantity = Mathf.Min(inventoryItem.Quantity, inventorySettings.MaxSlotQuantity);
		int remainingQuantity = inventoryItem.Quantity - slotQuantity;

		InventoryItem newItem = new InventoryItem(inventoryItem.Item, slotQuantity, slot);
		InventoryItem remainingItem = new InventoryItem(inventoryItem.Item, remainingQuantity, inventoryItem.Slot);
		items[slot] = newItem;

		OnUpdateInventoryUI?.Invoke(newItem, remainingItem);
		return true;
	}

	bool StackItem(InventoryItem inventoryItem, int slot)
	{
		// Check if this slot is already full
		InventoryItem existingItem = items[slot];
		int slotQuantity = Mathf.Min(inventoryItem.Quantity, inventorySettings.MaxSlotQuantity - existingItem.Quantity);
		if (slotQuantity <= 0) return false;

		// TODO test if inventoryItem is permanently changed as expected
		existingItem.Quantity += slotQuantity;
		int remainingQuantity = inventoryItem.Quantity - slotQuantity;
		InventoryItem remainingItem = new InventoryItem(inventoryItem.Item, remainingQuantity, inventoryItem.Slot);

		OnUpdateInventoryUI?.Invoke(existingItem, remainingItem);
		return true;
	}

	bool SwapItems(InventoryItem inventoryItem, int slot)
	{
		if (inventoryItem.Quantity > inventorySettings.MaxSlotQuantity) return false;

		InventoryItem existingItem = items[slot];
		existingItem.Slot = inventoryItem.Slot;

		// TODO test if the two separate items show as expected
		InventoryItem newItem = new InventoryItem(inventoryItem.Item, inventoryItem.Quantity, slot);
		items[slot] = newItem;

		OnUpdateInventoryUI?.Invoke(newItem, existingItem);
		return true;
	}
}
