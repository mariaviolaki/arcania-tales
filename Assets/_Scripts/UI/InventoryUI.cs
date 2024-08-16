using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : SlotContainerUI
{
	void Awake()
	{
		InitSlotListeners();	
	}

	public void SetOpen(bool isOpen)
	{
		gameObject.SetActive(isOpen);
	}
}
