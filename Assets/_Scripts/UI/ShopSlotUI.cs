using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSlotUI : MonoBehaviour, IPointerClickHandler
{
	[SerializeField] Image itemImage;
	[SerializeField] TMP_Text nameText;
	[SerializeField] TMP_Text amountText;
	[SerializeField] TMP_Text costText;

	PlayerWallet playerWallet;
	ShopItem shopItem;

	public Action<ShopSlotUI> OnSelectSlot;

	public ShopItem Product { get { return shopItem; } }

	void OnDestroy()
	{
		if (playerWallet != null)
		{
			playerWallet.OnGoldChange -= UpdateVisibility;
		}
	}

	public void SetPlayerWallet(PlayerWallet playerWallet)
	{
		this.playerWallet = playerWallet;
		playerWallet.OnGoldChange += UpdateVisibility;
	}

	public bool IsActive()
	{
		if (playerWallet == null || shopItem == null) return false;

		return playerWallet.Gold >= shopItem.Cost;
	}

	public void Fill(ShopItem shopItem)
	{
		this.shopItem = shopItem;

		itemImage.sprite = shopItem.Item.Image;
		nameText.text = shopItem.Item.Name;
		amountText.text = shopItem.Quantity.ToString();
		costText.text = GameUtils.FormatNumberString(shopItem.Cost);

		UpdateVisibility();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		OnSelectSlot?.Invoke(this);
	}

	void UpdateVisibility()
	{
		bool canAfford = IsActive();

		nameText.color = canAfford ? Color.black : Color.gray;
		costText.color = canAfford ? Color.black : Color.gray;
	}
}
