using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SharedInventoryItemUI : MonoBehaviour
{
	[SerializeField] InputHandlerSO inputHandler;
	[SerializeField] Image itemImage;
	[SerializeField] TMP_Text quantityText;

	InventoryItem sharedItem;

	public InventoryItem Item { get { return sharedItem; } }

	void Awake()
	{
		gameObject.SetActive(false);
		inputHandler.OnUIMoveInput += MoveItem;
		inputHandler.OnSelectInput += EnableUIControls;
	}

	public void ShowItem(InventoryItem inventoryItem, Vector2 position)
	{
		inputHandler.SetUIEnabled(true);
		gameObject.SetActive(true);

		sharedItem = inventoryItem;
		itemImage.sprite = inventoryItem.Item.Image;
		quantityText.text = inventoryItem.Quantity.ToString();
		transform.position = position;
	}

	public void ReleaseItem()
	{
		inputHandler.SetUIEnabled(false);
		sharedItem = null;
		gameObject.SetActive(false);
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
		bool hasUIControls = isUISelected && sharedItem != null;
		inputHandler.SetUIEnabled(hasUIControls);
	}
}
