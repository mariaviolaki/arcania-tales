using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenu : MonoBehaviour
{
	[SerializeField] GameObject inventory;
	[SerializeField] GameObject inventoryButton;

	void Start()
	{
		inventory.SetActive(false);
	}

	public void OpenInventory()
	{
		inventoryButton.SetActive(false);
		inventory.SetActive(true);
	}

	public void CloseInventory()
	{
		inventoryButton.SetActive(true);
		inventory.SetActive(false);
	}
}
