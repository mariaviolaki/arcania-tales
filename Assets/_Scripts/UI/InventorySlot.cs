using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
	[SerializeField] GameObject imageContainer;
	[SerializeField] GameObject textContainer;

	public Action<ItemSO, int, int, Vector2> OnSelectSlot;

	SlotContainerUI rootContainer;
	Image itemImage;
	TMP_Text quantityText;

	ItemSO item;
	int quantity;

	void Awake()
	{
		rootContainer = transform.root.GetComponent<SlotContainerUI>();
		itemImage = imageContainer.GetComponent<Image>();
		quantityText = textContainer.GetComponent<TMP_Text>();

		InitListeners();
	}

	public void Fill(ItemSO item, int quantity)
	{
		this.item = item;
		this.quantity = quantity;

		itemImage.sprite = item.Image;
		quantityText.text = quantity.ToString();

		imageContainer.SetActive(true);
		textContainer.SetActive(true);
	}

	public void Empty()
	{
		this.item = null;
		this.quantity = 0;

		itemImage.sprite = null;
		quantityText.text = "";

		imageContainer.SetActive(false);
		textContainer.SetActive(false);
	}

	void SelectSlot(PointerEventData eventData)
	{
		int slot = transform.GetSiblingIndex();
		OnSelectSlot?.Invoke(item, quantity, slot, eventData.position);
	}

	void InitListeners()
	{
		// When the image is clicked, select the item in the slot
		EventTrigger eventTrigger = GetComponent<EventTrigger>();
		EventTrigger.Entry clickEvent = new EventTrigger.Entry();
		clickEvent.eventID = EventTriggerType.PointerClick;
		clickEvent.callback.AddListener((data) => SelectSlot((PointerEventData)data));
		eventTrigger.triggers.Add(clickEvent);
	}
}
