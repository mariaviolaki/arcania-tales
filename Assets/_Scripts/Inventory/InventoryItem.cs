public class InventoryItem
{
	public ItemSO Item;
	public int Quantity;
	public int Slot;

	public InventoryItem(ItemSO item, int quantity, int slot)
	{
		this.Item = item;
		this.Quantity = quantity;
		this.Slot = slot;
	}
}
