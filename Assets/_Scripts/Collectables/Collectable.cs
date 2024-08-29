using UnityEngine;

public class Collectable : MonoBehaviour, IInteractable
{
	[SerializeField] SpriteRenderer spriteRenderer;

	InventoryManager inventoryManager;
	DateManager dateManager;
	ItemSO item;

	int interactionHour;

	public ItemSO Item { get { return item; } }

	void Awake()
	{
		interactionHour = -1;
	}

	public void Init(ItemSO item, InventoryManager inventoryManager, DateManager dateManager)
	{
		this.item = item;
		this.inventoryManager = inventoryManager;
		this.dateManager = dateManager;

		dateManager.OnHourPassed += CheckRespawnTime;
		spriteRenderer.sprite = item.Image;
	}

	void IInteractable.Interact(Transform player)
	{
		bool isCollected = inventoryManager.AddItem(item);
		if (isCollected)
		{
			RemoveCollectable();
		}
	}

	void CheckRespawnTime()
	{
		if (interactionHour == -1) return;

		int hoursPassed = dateManager.GetTotalHours();
		int respawnHour = interactionHour + item.RespawnHours;

		if (hoursPassed > respawnHour)
		{
			RespawnCollectable();
		}
	}

	void RemoveCollectable()
	{
		interactionHour = dateManager.GetTotalHours();
		gameObject.SetActive(false);
	}

	void RespawnCollectable()
	{
		interactionHour = -1;
		gameObject.SetActive(true);
	}
}
