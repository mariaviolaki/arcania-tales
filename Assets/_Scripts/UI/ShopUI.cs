using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.InputSystem;

public class ShopUI : CanvasUI
{
	[Header("External Dependencies")]
	[SerializeField] PlayerWallet playerWallet;
	[SerializeField] InventoryManager inventoryManager;
	[SerializeField] SelectedItemUI selectedItemUI;

	[Header("Shop Owner")]
	[SerializeField] Image ownerImage;
	[SerializeField] TMP_Text ownerText;
	[SerializeField] TMP_Text ownerDialogueText;

	[Header("Products List")]
	[SerializeField] Transform slotContainer;

	[Header("Product Details")]
	[SerializeField] Image itemImage;
	[SerializeField] TMP_Text itemText;
	[SerializeField] TMP_Text itemDescriptionText;
	[SerializeField] ButtonUI buyButton;
	[SerializeField] ButtonUI sellButton;
	[SerializeField] ButtonUI closeButton;
	[SerializeField] Color disabledBuyColor;

	ShopCounter currentShop;
	ShopSlotUI activeSlot;

	Image buyButtonImage;
	Color enabledBuyColor = Color.red;

	public Action OnCloseShopUI;

	public ShopCounter CurrentShop { get { return currentShop; } set { currentShop = value; } }

	override protected void Awake()
	{
		base.Awake();

		buyButtonImage = buyButton.GetComponent<Image>();
		enabledBuyColor = (buyButtonImage == null) ? enabledBuyColor : buyButtonImage.color;

		inventoryManager.OnInventoryFull += () => ShowOwnerDialogue(currentShop.GetOwnerInventoryWarning());
		selectedItemUI.OnShowSelectedItem += () => SetActiveSlot(null);
		buyButton.OnPointerClickUI += BuyItem;
		sellButton.OnPointerClickUI += SellItem;
		closeButton.OnPointerClickUI += () => SetCurrentShop(null);

		InitProductSlots();
		SetActiveSlot(null);
	}

	override protected void OnDestroy()
	{
		base.OnDestroy();

		inventoryManager.OnInventoryFull -= () => ShowOwnerDialogue(currentShop.GetOwnerInventoryWarning());
		selectedItemUI.OnShowSelectedItem -= () => SetActiveSlot(null);
		buyButton.OnPointerClickUI -= BuyItem;
		sellButton.OnPointerClickUI -= SellItem;
		closeButton.OnPointerClickUI -= () => SetCurrentShop(null);
	}

	public void SetCurrentShop(ShopCounter shopCounter)
	{
		if (shopCounter == null)
		{
			OnCloseShopUI?.Invoke();
			currentShop = shopCounter;
		}
		else
		{
			currentShop = shopCounter;
			LoadShop(shopCounter);
		}
	}

	void InitProductSlots()
	{
		foreach (Transform slot in slotContainer)
		{
			ShopSlotUI shopSlotUI = slot.GetComponent<ShopSlotUI>();
			if (shopSlotUI == null) continue;

			shopSlotUI.SetPlayerWallet(playerWallet);
			shopSlotUI.OnSelectSlot += SetActiveSlot;
		}
	}

	void SetActiveSlot(ShopSlotUI shopSlotUI)
	{
		activeSlot = shopSlotUI;

		if (activeSlot == null)
		{
			itemImage.color = Color.clear;
			itemText.text = "";
			itemDescriptionText.text = "";
			buyButtonImage.color = disabledBuyColor;
		}
		else
		{
			itemImage.sprite = activeSlot.Product.Item.Image;
			itemImage.color = Color.white;
			itemText.text = activeSlot.Product.Item.Name;
			itemDescriptionText.text = activeSlot.Product.Item.Description;
			buyButtonImage.color = activeSlot.IsActive() ? enabledBuyColor : disabledBuyColor;
		}
	}

	void LoadShop(ShopCounter shopCounter)
	{
		ownerText.text = shopCounter.GetOwner().NpcName;

		ShowOwnerDialogue(currentShop.GetOwnerGreeting());
		SetActiveSlot(null);
		FillShopItems(shopCounter);
	}

	void BuyItem()
	{
		if (activeSlot == null || !activeSlot.IsActive()) return;
		else if (playerWallet.Gold < activeSlot.Product.Cost)
		{
			SetActiveSlot(null);
			return;
		}

		bool isPurchased = inventoryManager.AutoAddItems(activeSlot.Product.Item, activeSlot.Product.Quantity);
		if (!isPurchased)
		{
			SetActiveSlot(null);
			return;
		}

		playerWallet.RemoveGold(activeSlot.Product.Cost);
		ShowOwnerDialogue(currentShop.GetOwnerPurchaseThanks());

		// The inventory isn't full, but the player can no longer afford this item
		if (activeSlot != null && !activeSlot.IsActive())
		{
			SetActiveSlot(null);
		}
	}

	void SellItem()
	{
		if (selectedItemUI.Item == null)
		{
			ShowOwnerDialogue(currentShop.GetOwnerSaleWarning());
			return;
		}

		playerWallet.AddGold(selectedItemUI.Item.Item.Cost);
		selectedItemUI.ReduceQuantity();
		ShowOwnerDialogue(currentShop.GetOwnerSaleThanks());
	}

	void FillShopItems(ShopCounter shopCounter)
	{
		List<ShopItem> items = shopCounter.GetShopItems();

		int slotIndex = 0;
		foreach (Transform slotTransform in slotContainer)
		{
			bool isActive = slotIndex++ < items.Count;
			slotTransform.gameObject.SetActive(isActive);
			if (!isActive) continue;

			ShopSlotUI slotUI = slotTransform.GetComponent<ShopSlotUI>();
			if (slotUI == null) continue;

			ShopItem shopItem = items[slotIndex - 1];
			slotUI.Fill(shopItem);
		}
	}

	void ShowOwnerDialogue(DialoguePart dialoguePart)
	{
		ownerImage.sprite = dialoguePart.GetSprite(currentShop.GetOwner());
		ownerDialogueText.text = dialoguePart.Text;
	}
}
