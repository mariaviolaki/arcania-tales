using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
	[SerializeField] GameSettingsSO gameSettings;

	public Action<InventoryItem, InventoryItem> OnAddCollectableToInventory;
	List<InventoryItem> items;

	void Awake()
	{
		int maxCapacity = gameSettings.ToolSlots + gameSettings.InventorySlots;
		items = new List<InventoryItem>(Enumerable.Repeat((InventoryItem) null, maxCapacity));
	}

	// Automatically add a collectable to the inventory without choosing its slot
	public bool AddCollectable(Collectable collectable, int quantity = 1)
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
				bool isSameItemSlot = inventoryItem.Item.Data.Name == collectable.Data.Name;
				bool isFreeSpaceSlot = inventoryItem.Quantity < gameSettings.MaxSlotQuantity;

				if (isSameItemSlot && isFreeSpaceSlot)
				{
					return StackCollectable(collectable, quantity, i);
				}
			}
		}

		return AddNewCollectable(collectable, quantity, firstAvailableSlot);
	}

	// Manually rearrange the inventory by forcing an item into a specific slot
	public bool AddCollectableToSlot(Collectable collectable, int slot, int quantity)
	{
		if (items[slot] == null)
		{
			return AddNewCollectable(collectable, quantity, slot);
		}
		else if (items[slot].Item.Data.Name == collectable.Data.Name)
		{
			return StackCollectable(collectable, quantity, slot);
		}
		else
		{
			return SwapCollectables(collectable, quantity, slot);
		}
	}

	bool AddNewCollectable(Collectable collectable, int quantity, int slot)
	{
		if (quantity <= 0)
		{
			OnAddCollectableToInventory?.Invoke(null, new InventoryItem(collectable, quantity, -1));
			return false;
		}

		// Ensure that the new item will not exceed the max slot quantity
		int slotQuantity = Mathf.Min(quantity, gameSettings.MaxSlotQuantity);

		InventoryItem newItem = new InventoryItem(collectable, slotQuantity, slot);
		InventoryItem remainingItem = new InventoryItem(collectable, quantity - slotQuantity, -1);
		items[slot] = newItem;

		OnAddCollectableToInventory?.Invoke(newItem, remainingItem);
		return true;
	}

	bool StackCollectable(Collectable collectable, int quantity, int slot)
	{
		// Check if this slot is already full
		InventoryItem inventoryItem = items[slot];
		int slotQuantity = Mathf.Min(quantity, gameSettings.MaxSlotQuantity - inventoryItem.Quantity);
		if (slotQuantity <= 0)
		{
			OnAddCollectableToInventory?.Invoke(null, new InventoryItem(collectable, quantity, -1));
			return false;
		}

		// TODO test if inventoryItem is permanently changed as expected
		inventoryItem.Quantity += slotQuantity;
		InventoryItem remainingItem = new InventoryItem(collectable, quantity - slotQuantity, -1);

		OnAddCollectableToInventory?.Invoke(inventoryItem, remainingItem);
		return true;
	}

	bool SwapCollectables(Collectable collectable, int quantity, int slot)
	{
		if (quantity <= 0 || quantity > gameSettings.MaxSlotQuantity)
		{
			OnAddCollectableToInventory?.Invoke(null, new InventoryItem(collectable, quantity, -1));
			return false;
		}

		InventoryItem existingItem = items[slot];
		existingItem.Slot = -1;

		// TODO test if the two separate items show as expected
		InventoryItem newItem = new InventoryItem(collectable, quantity, slot);
		items[slot] = newItem;

		OnAddCollectableToInventory?.Invoke(newItem, existingItem);
		return true;
	}
}
