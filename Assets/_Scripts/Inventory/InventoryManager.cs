using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StorageData
{
	public StorageChest Chest;
	public Vector2 Pos;
	public List<InventoryItem> Items;

	public StorageData(StorageChest chest, Vector2 pos, List<InventoryItem> items)
	{
		this.Chest = chest;
		this.Pos = pos;
		this.Items = items;
	}
}

public class InventoryManager : MonoBehaviour
{
	[SerializeField] InventorySettingsSO settings;
	[SerializeField] GameSceneManager sceneManager;

	List<InventoryItem> items;
	Dictionary<GameEnums.Scene, List<StorageData>> sceneStorages;

	public Action<InventoryItem, InventoryItem, StorageChest> OnUpdateInventoryUI;

	void Awake()
	{
		// The storage slots are stored separately for each scene
		sceneStorages = new Dictionary<GameEnums.Scene, List<StorageData>>();

		// The items contain all the toolbar and inventory slots
		int maxCapacity = settings.ToolSlots + settings.InventorySlots;
		items = new List<InventoryItem>(Enumerable.Repeat((InventoryItem)null, maxCapacity));
	}

	void Start()
	{
		sceneManager.OnEndChangeScene += (Vector2 playerPos) => UpdateSceneStorages();

		ReloadSceneChests();
	}

	void OnDestroy()
	{
		sceneManager.OnEndChangeScene -= (Vector2 playerPos) => UpdateSceneStorages();
	}

	void UpdateSceneStorages()
	{
		DeleteSceneChests();
		ReloadSceneChests();
	}

	public List<InventoryItem> GetStorageItems(StorageChest chest)
	{
		StorageData storageData = sceneStorages[sceneManager.CurrentScene].Find((storage) => storage.Chest == chest);
		return storageData.Items;
	}

	// Automatically add an item to the inventory without choosing its slot
	public bool AutoAddItem(ItemSO item, int quantity = 1)
	{
		int firstAvailableSlot = int.MaxValue;

		for (int i = 0; i < settings.ToolSlots + settings.InventorySlots; i++)
		{
			InventoryItem inventoryItem = items[i];

			// Save the first free slot in case this item cannot be stacked
			if (inventoryItem == null && i < firstAvailableSlot)
			{
				firstAvailableSlot = i;
			}
			// Attempt to stack the item in the first available slot
			else if (inventoryItem != null)
			{
				bool isSameItemSlot = inventoryItem.Item.Name == item.Name;
				bool isFreeSpaceSlot = inventoryItem.Quantity < settings.MaxSlotQuantity;

				if (isSameItemSlot && isFreeSpaceSlot)
				{
					return StackItem(items, new InventoryItem(item, quantity, -1), i, null);
				}
			}
		}

		return AddNewItem(items, new InventoryItem(item, quantity, -1), firstAvailableSlot, null);
	}

	// Manually rearrange the inventory by forcing an item into a specific slot
	public bool AddItemToSlot(InventoryItem inventoryItem, int slot, StorageChest chest = null)
	{
		if (inventoryItem == null || inventoryItem.Quantity <= 0 || slot < 0) return false;

		// Find a specific scene storage if the player selected one
		List<InventoryItem> container = chest == null ? items : GetSceneStorageItems(chest);
		if (container == null) return false;

		int maxCapacity = chest == null ? (settings.ToolSlots + settings.InventorySlots) : settings.StorageSlots;
		if (slot >= maxCapacity) return false;

		if (container[slot] == null)
		{
			return AddNewItem(container, inventoryItem, slot, chest);
		}
		else if (container[slot].Item.Name == inventoryItem.Item.Name)
		{
			return StackItem(container, inventoryItem, slot, chest);
		}
		else
		{
			return SwapItems(container, inventoryItem, slot, chest);
		}
	}

	public bool RemoveItemFromSlot(int slot, StorageChest chest = null)
	{
		if (slot < 0) return false;

		// Find a specific scene storage if the player selected one
		List<InventoryItem> container = chest == null ? items : GetSceneStorageItems(chest);
		if (container == null || container[slot] == null) return false;

		InventoryItem remainingItem = new InventoryItem(container[slot].Item, container[slot].Quantity, slot);
		container[slot] = null;

		OnUpdateInventoryUI?.Invoke(null, remainingItem, chest);
		return true;
	}

	bool AddNewItem(List<InventoryItem> container, InventoryItem inventoryItem, int slot, StorageChest chest)
	{
		// Ensure that the new item will not exceed the max slot quantity
		int slotQuantity = Mathf.Min(inventoryItem.Quantity, settings.MaxSlotQuantity);
		int remainingQuantity = inventoryItem.Quantity - slotQuantity;

		InventoryItem newItem = new InventoryItem(inventoryItem.Item, slotQuantity, slot);
		InventoryItem remainingItem = new InventoryItem(inventoryItem.Item, remainingQuantity, inventoryItem.Slot);
		container[slot] = newItem;

		OnUpdateInventoryUI?.Invoke(newItem, remainingItem, chest);
		return true;
	}

	bool StackItem(List<InventoryItem> container, InventoryItem inventoryItem, int slot, StorageChest chest)
	{
		// Check if this slot is already full
		InventoryItem existingItem = container[slot];
		int slotQuantity = Mathf.Min(inventoryItem.Quantity, settings.MaxSlotQuantity - existingItem.Quantity);
		if (slotQuantity <= 0) return false;

		existingItem.Quantity += slotQuantity;
		int remainingQuantity = inventoryItem.Quantity - slotQuantity;
		InventoryItem remainingItem = new InventoryItem(inventoryItem.Item, remainingQuantity, inventoryItem.Slot);

		OnUpdateInventoryUI?.Invoke(existingItem, remainingItem, chest);
		return true;
	}

	bool SwapItems(List<InventoryItem> container, InventoryItem inventoryItem, int slot, StorageChest chest)
	{
		if (inventoryItem.Quantity > settings.MaxSlotQuantity) return false;

		InventoryItem existingItem = container[slot];
		existingItem.Slot = inventoryItem.Slot;

		InventoryItem newItem = new InventoryItem(inventoryItem.Item, inventoryItem.Quantity, slot);
		container[slot] = newItem;

		OnUpdateInventoryUI?.Invoke(newItem, existingItem, chest);
		return true;
	}

	void DeleteSceneChests()
	{
		if (sceneManager.LastScene == GameEnums.Scene.None) return;

		foreach (StorageData storageData in sceneStorages[sceneManager.LastScene])
		{
			storageData.Chest = null;
		}
	}

	void ReloadSceneChests()
	{
		StorageChest[] storageChests = FindObjectsOfType<StorageChest>();

		if (!sceneStorages.ContainsKey(sceneManager.CurrentScene))
		{
			sceneStorages.Add(sceneManager.CurrentScene, new List<StorageData>());
		}

		foreach (StorageChest storageChest in storageChests)
		{
			Vector2 sceneStoragePos = (Vector2)storageChest.transform.position;
			bool isNewStorage = UpdateSceneChests(sceneStoragePos, storageChest);
			if (isNewStorage)
			{
				CreateNewStorage(sceneStoragePos, storageChest);
			}
		}
	}

	bool UpdateSceneChests(Vector2 sceneStoragePos, StorageChest storageChest)
	{
		bool isNewStorage = true;

		// Update the storage chest references for old chests
		foreach (StorageData storageInventoryData in sceneStorages[sceneManager.CurrentScene])
		{
			if (sceneStoragePos != storageInventoryData.Pos) continue;

			isNewStorage = false;
			storageInventoryData.Chest = storageChest;
			break;
		}

		return isNewStorage;
	}

	void CreateNewStorage(Vector2 sceneStoragePos, StorageChest storageChest)
	{
		// This storage is being created for the first time - this is a new scene
		List<InventoryItem> storageItems = new List<InventoryItem>(Enumerable.Repeat((InventoryItem)null, settings.StorageSlots));
		StorageData storageInventoryData = new StorageData(storageChest, sceneStoragePos, storageItems);
		sceneStorages[sceneManager.CurrentScene].Add(storageInventoryData);
	}

	List<InventoryItem> GetSceneStorageItems(StorageChest chest)
	{
		StorageData storageData = sceneStorages[sceneManager.CurrentScene].Find((storage) => storage.Chest == chest);
		return storageData.Items;
	}
}
