using UnityEngine;

[CreateAssetMenu(fileName = "InventorySettings", menuName = "Scriptable Objects/Inventory Settings")]
public class InventorySettingsSO : ScriptableObject
{
	[SerializeField] int toolSlots;
	[SerializeField] int inventorySlots;
	[SerializeField] int storageSlots;
	[SerializeField] int maxSlotQuantity;

	public int ToolSlots { get { return toolSlots; } }
	public int InventorySlots { get { return inventorySlots; } }
	public int StorageSlots { get { return storageSlots; } }
	public int MaxSlotQuantity { get { return maxSlotQuantity; } }
}
