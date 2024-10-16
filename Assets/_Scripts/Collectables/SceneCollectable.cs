using System;
using UnityEngine;

public class SceneCollectable : MonoBehaviour, IInteractable
{
	[SerializeField] SpriteRenderer spriteRenderer;

	InventoryManager inventoryManager;
	ItemSO item;

	public Action OnCollectableInteract;

	public ItemSO Item { get { return item; } }

	public void Init(InventoryManager inventoryManager, ItemSO item)
	{
		this.inventoryManager = inventoryManager;
		this.item = item;

		spriteRenderer.sprite = item.Image;
	}

	void IInteractable.Interact(Transform player)
	{
		bool isCollected = inventoryManager.AutoAddItem(item);
		if (isCollected)
		{
			gameObject.SetActive(false);
			OnCollectableInteract?.Invoke();
		}
	}
}
