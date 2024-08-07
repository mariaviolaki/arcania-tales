using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : SlotContainerUI
{
	[SerializeField] GameObject closeArea;

	UIManager uiManager;

	public void InitListeners(UIManager uiManager)
	{
		this.uiManager = uiManager;

		closeArea.GetComponent<Button>().onClick.AddListener(() => uiManager.SetInventoryOpen(false));
	}

	public void SetOpen(bool isOpen)
	{
		gameObject.SetActive(isOpen);
	}
}
