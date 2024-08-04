using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour
{
	[SerializeField] GameObject inventory;
	[SerializeField] GameObject inventoryButton;
	[SerializeField] GameObject closeArea;

	void Start()
	{
		CloseInventory();
	}

	public void OpenInventory()
	{
		inventory.SetActive(true);
		inventoryButton.SetActive(false);
		closeArea.SetActive(true);
	}

	public void CloseInventory()
	{
		inventory.SetActive(false);
		inventoryButton.SetActive(true);
		closeArea.SetActive(false);
	}
}
