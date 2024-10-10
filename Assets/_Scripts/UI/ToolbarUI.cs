using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolbarUI : SlotContainerUI
{
	[SerializeField] GameObject inventoryButton;
	[SerializeField] Sprite openInventorySprite;
	[SerializeField] Sprite closeInventorySprite;

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
			inventoryImage.sprite = uiManager.CurrentState == GameEnums.UIState.Toolbar ? closeInventorySprite : openInventorySprite;
		}
	}
}
