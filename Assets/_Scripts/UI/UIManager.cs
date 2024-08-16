using UnityEngine;

public class UIManager: MonoBehaviour
{
	[SerializeField] GameSettingsSO gameSettings;
	[SerializeField] InputHandlerSO inputHandler;
	[SerializeField] ToolbarUI toolbarUI;
	[SerializeField] InventoryUI inventoryUI;
	[SerializeField] SharedInventoryItemUI sharedInventoryItem;

	InventoryManager inventoryManager;
	Vector2 lastSelectedPosition;

	void Awake()
	{
		InitListeners();
	}

	void Start()
	{
		inventoryManager = FindObjectOfType<InventoryManager>();
		if (inventoryManager != null)
		{
			inventoryManager.OnUpdateInventoryUI += UpdateInventoryUI;
		}

		SetInventoryOpen(false);
	}

	public void ToggleInventory()
	{
		bool isOpen = inventoryUI.gameObject.activeInHierarchy;
		SetInventoryOpen(!isOpen);
	}

	void SetInventoryOpen(bool isOpen)
	{
		inputHandler.SetGameplayEnabled(!isOpen);

		toolbarUI.SetOpen(!isOpen);
		inventoryUI.SetOpen(isOpen);
	}

	void InitListeners()
	{
		// Subscribe to events triggered inside the containers
		toolbarUI.OnSelectFullSlot += AddInventoryItemToSlot;
		inventoryUI.OnSelectFullSlot += AddInventoryItemToSlot;
		toolbarUI.OnSelectEmptySlot += AddInventoryItemToSlot;
		inventoryUI.OnSelectEmptySlot += AddInventoryItemToSlot;

		// Setup actions that will open and close the different canvases
		toolbarUI.InitInventoryToggle(this);
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
		if (inventoryItem.Slot < gameSettings.ToolSlots)
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
		if (slot < gameSettings.ToolSlots)
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
			sharedInventoryItem.ShowItem(remainingItem, lastSelectedPosition);
		}
		else
		{
			sharedInventoryItem.ReleaseItem();
		}
	}

	void AddInventoryItemToSlot(int slot, Vector2 position)
	{
		lastSelectedPosition = position;

		if (!inventoryManager.AddItemToSlot(sharedInventoryItem.Item, slot))
		{
			RemoveInventoryItemFromSlot(slot);
		}
	}

	void RemoveInventoryItemFromSlot(int slot)
	{
		if (sharedInventoryItem.Item != null) return;

		if (inventoryManager.RemoveItemFromSlot(slot))
		{
			EmptySlot(slot);
		}
	}
}
