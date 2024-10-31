using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopCounter : MonoBehaviour, IInteractable
{
	[SerializeField] GameplaySettingsSO settings;
	[SerializeField] ShopSO shop;
	[SerializeField] SpriteRenderer spriteRenderer;

	UIManager uiManager;
	NpcMovement ownerMovement;

	void Start()
	{
		uiManager = FindObjectOfType<UIManager>();
		if (uiManager != null)
		{
			uiManager.Shop.OnCloseShopUI += CloseShop;
		}
	}

	void OnDestroy()
	{
		if (uiManager != null)
		{
			uiManager.Shop.OnCloseShopUI -= CloseShop;
		}
	}

	public List<ShopItem> GetShopItems() { return shop.Items; }
	public NpcSO GetOwner() { return shop.Owner; }
	public DialoguePart GetOwnerGreeting() { return GetOwnerDialogue(shop.OwnerDialogues.GetShopGreeting()); }
	public DialoguePart GetOwnerPurchaseThanks() { return GetOwnerDialogue(shop.OwnerDialogues.GetShopPurchaseThanks()); }
	public DialoguePart GetOwnerSaleThanks() { return GetOwnerDialogue(shop.OwnerDialogues.GetShopSaleThanks()); }
	public DialoguePart GetOwnerInventoryWarning() { return GetOwnerDialogue(shop.OwnerDialogues.GetShopInventoryWarning()); }
	public DialoguePart GetOwnerSaleWarning() { return GetOwnerDialogue(shop.OwnerDialogues.GetShopSaleWarning()); }

	public void Interact(Transform player)
	{
		if (uiManager == null) return;

		// Don't open the shop if its owner is not nearby
		NpcMovement ownerMovement = GetOwnerMovement();

		if (ownerMovement == null)
		{
			string hintText = "The shop owner doesn't seem to be here. Please come back later.";
			uiManager.Hint.ShowHint(hintText);
		}
		else
		{
			// Pause the current movement schedule and face the player
			this.ownerMovement = ownerMovement;
			Vector2 facingDirection = (player.position - transform.position).normalized;
			ownerMovement.SetSchedulePaused(true, facingDirection);

			uiManager.OpenShop(this);
		}
	}

	void CloseShop()
	{
		// Only close this particular shop the player is interacting with
		if (uiManager.Shop.CurrentShop != this || ownerMovement == null) return;

		ownerMovement.SetSchedulePaused(false);
		ownerMovement = null;
	}

	NpcMovement GetOwnerMovement()
	{
		// Search for npc colliders in the area around the counter
		Vector2 searchCenter = spriteRenderer.bounds.center;
		Vector2 searchDimensions = spriteRenderer.bounds.size * settings.ShopInteractDistance;
		int npcLayers = LayerMask.GetMask(GameConstants.CollisionLayers.Npc);

		// Search for the owner of this particular shop among the npc colliders
		Collider2D[] npcColliders = Physics2D.OverlapBoxAll(searchCenter, searchDimensions, 0, npcLayers);
		foreach (Collider2D npcCollider in npcColliders)
		{
			NpcMovement npcMovement = npcCollider.GetComponent<NpcMovement>();
			if (!npcMovement) continue;

			if (npcMovement.NpcInfo.NpcName == shop.Owner.NpcName) return npcMovement;
		}

		return null;
	}

	DialoguePart GetOwnerDialogue(DialogueSequence dialogue)
	{
		if (dialogue == null) return null;
		else return dialogue.DialogueParts[0];
	}
}
