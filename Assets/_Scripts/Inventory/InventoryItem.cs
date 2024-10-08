public class InventoryItem
{
	ItemSO item;
	int quantity;
	int slot;

	public InventoryItem(ItemSO item, int quantity, int slot)
	{
		this.item = item;
		this.quantity = quantity;
		this.slot = slot;
	}

	public ItemSO Item
	{
		get { return item; }
		set { item = value; }
	}

	public int Quantity
	{
		get { return quantity; }
		set { quantity = value; }
	}

	public int Slot
	{
		get { return slot; }
		set { slot = value; }
	}
}
