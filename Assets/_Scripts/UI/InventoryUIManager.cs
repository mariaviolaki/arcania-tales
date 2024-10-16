using Unity.VisualScripting;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
	[SerializeField] InventorySettingsSO inventorySettings;
	[SerializeField] InventoryManager inventoryManager;

	[SerializeField] ToolbarUI toolbarUI;
	[SerializeField] InventoryUI inventoryUI;
	[SerializeField] StorageUI storageUI;
	[SerializeField] SelectedItemUI selectedItemUI;

	Vector2 lastSelectedPosition;

	void Start()
	{
		InitListeners();
	}

	void OnDestroy()
	{
		inventoryManager.OnUpdateInventoryUI -= UpdateInventoryUI;

		toolbarUI.OnSelectFullSlot -= AddInventoryItemToSlot;
		toolbarUI.OnSelectEmptySlot -= AddInventoryItemToSlot;
		inventoryUI.OnSelectFullSlot -= AddInventoryItemToSlot;
		inventoryUI.OnSelectEmptySlot -= AddInventoryItemToSlot;
		storageUI.OnSelectFullStorageSlot -= AddItemToSlot;
		storageUI.OnSelectEmptyStorageSlot -= AddItemToSlot;
	}

	void InitListeners()
	{
		// Update the UI whenever something is changed in the inventory
		inventoryManager.OnUpdateInventoryUI += UpdateInventoryUI;

		// Subscribe to events triggered inside the containers
		toolbarUI.OnSelectFullSlot += AddInventoryItemToSlot;
		toolbarUI.OnSelectEmptySlot += AddInventoryItemToSlot;
		inventoryUI.OnSelectFullSlot += AddInventoryItemToSlot;
		inventoryUI.OnSelectEmptySlot += AddInventoryItemToSlot;
		storageUI.OnSelectFullStorageSlot += AddItemToSlot;
		storageUI.OnSelectEmptyStorageSlot += AddItemToSlot;
	}

	void UpdateInventoryUI(InventoryItem newItem, InventoryItem remainingItem, StorageChest chest)
	{
		if (newItem != null)
		{
			FillSlot(newItem, chest);
		}
		if (remainingItem != null)
		{
			SaveSharedInventoryItem(remainingItem, chest);
		}
	}

	void FillSlot(InventoryItem inventoryItem, StorageChest chest)
	{
		if (chest != null)
		{
			storageUI.FillSlot(inventoryItem, inventoryItem.Slot);
		}
		else if (inventoryItem.Slot < inventorySettings.ToolSlots)
		{
			toolbarUI.FillSlot(inventoryItem, inventoryItem.Slot);
		}
		else // The inventory slots are handled as the toolbar's continuation
		{
			inventoryUI.FillSlot(inventoryItem, inventoryItem.Slot - inventorySettings.ToolSlots);
		}
	}

	void EmptySlot(int slot, StorageChest chest)
	{
		if (chest != null)
		{
			storageUI.EmptySlot(slot);
		}
		else if (slot < inventorySettings.ToolSlots)
		{
			toolbarUI.EmptySlot(slot);
		}
		else // The inventory slots are handled as the toolbar's continuation
		{
			inventoryUI.EmptySlot(slot - inventorySettings.ToolSlots);
		}
	}

	void SaveSharedInventoryItem(InventoryItem remainingItem, StorageChest chest)
	{
		if (remainingItem.Quantity > 0)
		{
			selectedItemUI.ShowItem(remainingItem, lastSelectedPosition);
		}
		else
		{
			selectedItemUI.ReleaseItem();
		}
	}

	void AddInventoryItemToSlot(int slot, Vector2 position)
	{
		AddItemToSlot(slot, position);
	}

	void AddItemToSlot(int slot, Vector2 position, StorageChest chest = null)
	{
		lastSelectedPosition = position;

		if (!inventoryManager.AddItemToSlot(selectedItemUI.Item, slot, chest))
		{
			RemoveInventoryItemFromSlot(slot, chest);
		}
	}

	void RemoveInventoryItemFromSlot(int slot, StorageChest chest = null)
	{
		if (selectedItemUI.Item != null) return;

		if (inventoryManager.RemoveItemFromSlot(slot, chest))
		{
			EmptySlot(slot, chest);
		}
	}
}
