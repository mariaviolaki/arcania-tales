using UnityEngine;

public class InventoryUIManager
{
	InventorySettingsSO inventorySettings;
	ToolbarUI toolbarUI;
	InventoryUI inventoryUI;
	SharedInventoryItemUI sharedInventoryItemUI;

	InventoryManager inventoryManager;
	Vector2 lastSelectedPosition;

	public InventoryUIManager(InventorySettingsSO settings, ToolbarUI toolbar, InventoryUI inventory, SharedInventoryItemUI sharedItem)
	{
		this.inventorySettings = settings;
		this.toolbarUI = toolbar;
		this.inventoryUI = inventory;
		this.sharedInventoryItemUI = sharedItem;
		inventoryManager = GameObject.FindObjectOfType<InventoryManager>();

		InitListeners();
	}

	void InitListeners()
	{
		// Update the UI whenever something is changed in the inventory
		inventoryManager.OnUpdateInventoryUI += UpdateInventoryUI;

		// Subscribe to events triggered inside the containers
		toolbarUI.OnSelectFullSlot += AddInventoryItemToSlot;
		inventoryUI.OnSelectFullSlot += AddInventoryItemToSlot;
		toolbarUI.OnSelectEmptySlot += AddInventoryItemToSlot;
		inventoryUI.OnSelectEmptySlot += AddInventoryItemToSlot;
	}

	void UpdateInventoryUI(InventoryItem newItem, InventoryItem remainingItem)
	{
		if (newItem != null)
		{
			FillSlot(newItem);
		}
		if (remainingItem != null)
		{
			SaveSharedInventoryItem(remainingItem);
		}
	}

	void FillSlot(InventoryItem inventoryItem)
	{
		if (inventoryItem.Slot < inventorySettings.ToolSlots)
		{
			toolbarUI.FillSlot(inventoryItem);
		}
		else
		{
			inventoryUI.FillSlot(inventoryItem);
		}
	}

	void EmptySlot(int slot)
	{
		if (slot < inventorySettings.ToolSlots)
		{
			toolbarUI.EmptySlot(slot);
		}
		else
		{
			inventoryUI.EmptySlot(slot);
		}
	}

	void SaveSharedInventoryItem(InventoryItem remainingItem)
	{
		if (remainingItem.Quantity > 0)
		{
			sharedInventoryItemUI.ShowItem(remainingItem, lastSelectedPosition);
		}
		else
		{
			sharedInventoryItemUI.ReleaseItem();
		}
	}

	void AddInventoryItemToSlot(int slot, Vector2 position)
	{
		lastSelectedPosition = position;

		if (!inventoryManager.AddItemToSlot(sharedInventoryItemUI.Item, slot))
		{
			RemoveInventoryItemFromSlot(slot);
		}
	}

	void RemoveInventoryItemFromSlot(int slot)
	{
		if (sharedInventoryItemUI.Item != null) return;

		if (inventoryManager.RemoveItemFromSlot(slot))
		{
			EmptySlot(slot);
		}
	}
}
