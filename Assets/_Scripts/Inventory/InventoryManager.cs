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
	enum SlotState
	{
		None, Available, Stack
	}

	[SerializeField] InventorySettingsSO settings;
	[SerializeField] GameSceneManager sceneManager;

	List<InventoryItem> items;
	Dictionary<GameEnums.Scene, List<StorageData>> sceneStorages;

	public Action<InventoryItem, InventoryItem, StorageChest> OnUpdateInventoryUI;
	public Action OnInventoryFull;

	public List<InventoryItem> Items { get { return items; } }

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
	public bool AutoAddItem(ItemSO item)
	{
		int firstAvailableSlot = int.MaxValue;

		for (int slot = 0; slot < settings.ToolSlots + settings.InventorySlots; slot++)
		{
			SlotState slotState = GetAutoSlotState(firstAvailableSlot, slot, item);
			if (slotState == SlotState.Available)
			{
				firstAvailableSlot = slot;
			}
			else if (slotState == SlotState.Stack)
			{
				return StackItem(items, new InventoryItem(item, 1, -1), slot, null, false);
			}
		}

		return AddNewItem(items, new InventoryItem(item, 1, -1), firstAvailableSlot, null, false);
	}

	// Automatically add multiple items to the inventory without choosing their slots
	public bool AutoAddItems(ItemSO item, int quantity)
	{
		int startQuantity = quantity;

		while (quantity > 0)
		{
			int firstAvailableSlot = int.MaxValue;
			bool isStacked = false;

			for (int slot = 0; slot < settings.ToolSlots + settings.InventorySlots; slot++)
			{
				SlotState slotState = GetAutoSlotState(firstAvailableSlot, slot, item);
				if (slotState == SlotState.Available)
				{
					firstAvailableSlot = slot;
				}
				else if (slotState == SlotState.Stack)
				{
					InventoryItem inventoryItem = items[slot];
					int slotQuantity = Math.Min(quantity, settings.MaxSlotQuantity - inventoryItem.Quantity);
					isStacked = StackItem(items, new InventoryItem(item, slotQuantity, -1), slot, null, false);
					quantity -= isStacked ? slotQuantity : 0;
					break;
				}
			}

			// Add the item to the first available slot only if it couldn't be stacked into another
			if (!isStacked)
			{
				// All the slots are full and cannot be stacked
				if (firstAvailableSlot == int.MaxValue) break;

				int slotQuantity = Math.Min(quantity, settings.MaxSlotQuantity);
				bool isAdded = AddNewItem(items, new InventoryItem(item, slotQuantity, -1), firstAvailableSlot, null, false);
				quantity -= isAdded ? slotQuantity : 0;
			}
		}

		if (quantity > 0)
		{
			// Remove any already added items if there is no space for the rest
			AutoRemoveItem(item, startQuantity - quantity);
			OnInventoryFull?.Invoke();
			return false;
		}

		return true;
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
			return AddNewItem(container, inventoryItem, slot, chest, true);
		}
		else if (container[slot].Item.Name == inventoryItem.Item.Name)
		{
			return StackItem(container, inventoryItem, slot, chest, true);
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

	// Automatically removes a type of item from any slots that contain it
	bool AutoRemoveItem(ItemSO item, int quantity)
	{
		// Remove items from the last slots first
		for (int slot = items.Count - 1; slot >= 0; slot--)
		{
			// Only process the slots that contain the item specified 
			InventoryItem slotItem = items[slot];
			if (slotItem == null || slotItem.Item.Name != item.Name) continue;

			int quantityRemoved = Mathf.Clamp(quantity, 1, slotItem.Quantity);
			quantity -= quantityRemoved;

			InventoryItem inventoryItem;

			// Delete the item entry entirely if the new quantity is 0
			if (quantityRemoved == slotItem.Quantity)
			{
				items[slot] = null;
				inventoryItem = new InventoryItem(null, 0, slot);
			}
			else
			{
				items[slot].Quantity -= quantityRemoved;
				inventoryItem = new InventoryItem(item, items[slot].Quantity, slot);
			}
			
			// Update the UI after removing the item
			OnUpdateInventoryUI?.Invoke(inventoryItem, null, null);

			// The requested items were removed, return early
			if (quantity == 0) return true;
		}

		return false;
	}

	bool AddNewItem(List<InventoryItem> container, InventoryItem inventoryItem, int slot, StorageChest chest, bool isRemainingAllowed)
	{
		// Ensure that the new item will not exceed the max slot quantity
		int slotQuantity = Mathf.Min(inventoryItem.Quantity, settings.MaxSlotQuantity);
		int remainingQuantity = inventoryItem.Quantity - slotQuantity;

		if (!isRemainingAllowed && remainingQuantity > 0) return false;

		InventoryItem newItem = new InventoryItem(inventoryItem.Item, slotQuantity, slot);
		InventoryItem remainingItem = new InventoryItem(inventoryItem.Item, remainingQuantity, inventoryItem.Slot);
		container[slot] = newItem;

		OnUpdateInventoryUI?.Invoke(newItem, remainingItem, chest);
		return true;
	}

	bool StackItem(List<InventoryItem> container, InventoryItem inventoryItem, int slot, StorageChest chest, bool isRemainingAllowed)
	{
		// Check if this slot is already full
		InventoryItem existingItem = container[slot];
		int slotQuantity = Mathf.Min(inventoryItem.Quantity, settings.MaxSlotQuantity - existingItem.Quantity);
		if (slotQuantity <= 0) return false;

		int remainingQuantity = inventoryItem.Quantity - slotQuantity;
		if (!isRemainingAllowed && remainingQuantity > 0) return false;

		existingItem.Quantity += slotQuantity;
		InventoryItem remainingItem = !isRemainingAllowed ? null : new InventoryItem(inventoryItem.Item, remainingQuantity, inventoryItem.Slot);

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

	SlotState GetAutoSlotState(int firstAvailableSlot, int slot, ItemSO targetItem)
	{
		InventoryItem inventoryItem = items[slot];

		// Save the first free slot in case this item cannot be stacked
		if (inventoryItem == null && slot < firstAvailableSlot) return SlotState.Available;
		else if (inventoryItem == null) return SlotState.None;

		// Attempt to stack the item in the first available slot with the same item
		bool isSameItemSlot = inventoryItem.Item.Name == targetItem.Name;
		bool isFreeSpaceSlot = inventoryItem.Quantity < settings.MaxSlotQuantity;

		if (isSameItemSlot && isFreeSpaceSlot) return SlotState.Stack;
		else return SlotState.None;
	}
}
