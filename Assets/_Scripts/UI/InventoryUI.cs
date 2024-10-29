using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : SlotContainerUI
{
	protected override int GetStartSlot()
	{
		// The inventory slots are handled as the toolbar's continuation
		return inventorySettings.ToolSlots;
	}

	protected override int GetEndSlot()
	{
		// The inventory slots are handled as the toolbar's continuation
		return inventorySettings.ToolSlots + inventorySettings.InventorySlots - 1;
	}
}
