using System.Collections.Generic;
using UnityEngine;

public class SlotContainerUI : MonoBehaviour
{
	[SerializeField] GameSettingsSO gameSettings;
	[SerializeField] List<InventorySlot> slots;

    public void FillSlot(InventoryItem inventoryItem)
	{
		if (inventoryItem.Quantity <= 0) return;

		// Since the toolbar is always present, all other containers start after its last index
		int slotIndex = inventoryItem.Slot;
		if (this.GetType() != typeof(ToolbarUI))
		{
			slotIndex -= gameSettings.ToolSlots;
		}

		CollectableSO collectableData = inventoryItem.Item.Data;
		slots[slotIndex].Fill(collectableData.Image, inventoryItem.Quantity);
	}
}
