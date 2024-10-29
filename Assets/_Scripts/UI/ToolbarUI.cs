using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolbarUI : SlotContainerUI
{
	[SerializeField] GameObject inventoryButton;
	[SerializeField] Sprite toolbarSprite;
	[SerializeField] Sprite inventorySprite;
	[SerializeField] Sprite storageSprite;

	Image inventoryImage;
	UIManager uiManager;

	public GameObject InventoryButton { get { return inventoryButton; } }

	public void SetUIManager(UIManager uiManager) { this.uiManager = uiManager; }

	override protected void Awake()
	{
		base.Awake();
		inventoryImage = inventoryButton.GetComponent<Image>();
	}

	override public void SetActive(bool isActive)
	{
		base.SetActive(isActive);

		if (inventoryImage != null && uiManager != null)
		{
			if (uiManager.CurrentState == GameEnums.UIState.Toolbar)
			{
				inventoryImage.sprite = toolbarSprite;
			}
			else if (uiManager.CurrentState == GameEnums.UIState.Inventory)
			{
				inventoryImage.sprite = inventorySprite;
			}
			else if(uiManager.CurrentState == GameEnums.UIState.Storage)
			{
				inventoryImage.sprite = storageSprite;
			}
		}
	}

	protected override int GetStartSlot()
	{
		return 0;
	}

	protected override int GetEndSlot()
	{
		return inventorySettings.ToolSlots - 1;
	}
}
