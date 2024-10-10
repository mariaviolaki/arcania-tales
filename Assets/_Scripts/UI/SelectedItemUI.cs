using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectedItemUI : CanvasUI
{
	[SerializeField] InputHandlerSO inputHandler;
	[SerializeField] Image itemImage;
	[SerializeField] TMP_Text quantityText;

	InventoryItem selectedItem;

	public Action OnShowSelectedItem;
	public Action OnReleaseSelectedItem;

	public InventoryItem Item { get { return selectedItem; } }

	override protected void Awake()
	{
		base.Awake();
		inputHandler.OnUIMoveInput += MoveItem;
		inputHandler.OnSelectInput += EnableUIControls;
	}

	public void ShowItem(InventoryItem inventoryItem, Vector2 position)
	{
		inputHandler.SetUIEnabled(true);

		selectedItem = inventoryItem;
		itemImage.sprite = inventoryItem.Item.Image;
		quantityText.text = inventoryItem.Quantity.ToString();
		transform.position = position;

		OnShowSelectedItem?.Invoke();
	}

	public void ReleaseItem()
	{
		if (selectedItem == null) return;

		inputHandler.SetUIEnabled(false);
		selectedItem = null;

		OnReleaseSelectedItem?.Invoke();
	}

	void MoveItem(Vector2 position)
	{
		if (gameObject.activeInHierarchy)
		{
			transform.position = position;
		}
	}

	void EnableUIControls(bool isUISelected)
	{
		bool isHoldingItem = isUISelected && selectedItem != null;
		if (isHoldingItem)
		{
			inputHandler.SetUIEnabled(isHoldingItem);
		}
	}
}
