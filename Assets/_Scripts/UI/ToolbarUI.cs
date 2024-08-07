using UnityEngine;
using UnityEngine.UI;

public class ToolbarUI : SlotContainerUI
{
	[SerializeField] GameObject inventoryButton;

	UIManager uiManager;

	public void InitListeners(UIManager uiManager)
	{
		this.uiManager = uiManager;

		inventoryButton.GetComponent<Button>().onClick.AddListener(() => uiManager.SetInventoryOpen(true));
	}

	public void SetOpen(bool isOpen)
	{
		inventoryButton.SetActive(isOpen);
	}
}
