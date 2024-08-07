using UnityEngine;

public class UIManager: MonoBehaviour
{
	[SerializeField] GameSettingsSO gameSettings;
	[SerializeField] ToolbarUI toolbarUI;
	[SerializeField] InventoryUI inventoryUI;

	InventoryManager inventoryManager;

	void Awake()
	{
		InitListeners();

		SetInventoryOpen(false);
	}

	void Start()
	{
		inventoryManager = FindObjectOfType<InventoryManager>();
		if (inventoryManager != null)
		{
			inventoryManager.OnAddCollectableToInventory += AddCollectableToInventory;
		}
	}

	void InitListeners()
	{
		toolbarUI.InitListeners(this);
		inventoryUI.InitListeners(this);
	}

	public void SetInventoryOpen(bool isOpen)
	{
		toolbarUI.SetOpen(!isOpen);
		inventoryUI.SetOpen(isOpen);
	}

	void AddCollectableToInventory(InventoryItem newItem, InventoryItem remainingItem)
	{
		if (newItem.Slot < gameSettings.ToolSlots)
		{
			toolbarUI.FillSlot(newItem);
		}
		else
		{
			inventoryUI.FillSlot(newItem);
		}
	}
}
