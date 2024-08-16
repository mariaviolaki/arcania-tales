using UnityEngine;

public class Collectable : MonoBehaviour, IInteractable
{
	[SerializeField] CollectableSO data;

	InventoryManager inventoryManager;

	public CollectableSO Data { get { return data; } }

	void Start()
	{
		inventoryManager = FindObjectOfType<InventoryManager>();
	}

	void IInteractable.Interact(Transform player)
	{
		if (inventoryManager == null) return;

		bool isCollected = inventoryManager.AddCollectable(data);
		if (isCollected)
		{
			Destroy(gameObject);
		}
	}
}
