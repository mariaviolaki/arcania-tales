using UnityEngine;

public class UIManager: MonoBehaviour
{
	[SerializeField] InventorySettingsSO inventorySettings;
	[SerializeField] InputHandlerSO inputHandler;
	[SerializeField] ToolbarUI toolbarUI;
	[SerializeField] InventoryUI inventoryUI;
	[SerializeField] SharedInventoryItemUI sharedInventoryItemUI;

	InventoryUIManager inventoryUIManager;

	void Start()
	{
		inventoryUIManager = new InventoryUIManager(inventorySettings, toolbarUI, inventoryUI, sharedInventoryItemUI);

		InitListeners();
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
		// Setup actions that will open and close the different canvases
		toolbarUI.InitInventoryToggle(this);
	}
}
