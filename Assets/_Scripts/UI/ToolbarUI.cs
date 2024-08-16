using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolbarUI : SlotContainerUI
{
	[SerializeField] GameObject inventoryButton;
	[SerializeField] Sprite openInventorySprite;
	[SerializeField] Sprite closeInventorySprite;

	UIManager uiManager;
	Image inventoryImage;

	void Awake()
	{
		inventoryImage = inventoryButton.GetComponent<Image>();

		InitSlotListeners();
	}

	public void InitInventoryToggle(UIManager uiManager)
	{
		this.uiManager = uiManager;

		// When the image is clicked, open or close the inventory
		EventTrigger eventTrigger = inventoryButton.GetComponent<EventTrigger>();
		EventTrigger.Entry clickEvent = new EventTrigger.Entry();
		clickEvent.eventID = EventTriggerType.PointerClick;
		clickEvent.callback.AddListener((data) => ToggleInventory((PointerEventData)data));
		eventTrigger.triggers.Add(clickEvent);
	}

	public void SetOpen(bool isOpen)
	{
		inventoryImage.sprite = isOpen ? closeInventorySprite : openInventorySprite;
	}

	void ToggleInventory(PointerEventData eventData)
	{
		uiManager.ToggleInventory();
	}
}
