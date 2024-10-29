using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shop", menuName = "Scriptable Objects/Shop")]
public class ShopSO : ScriptableObject
{
	[SerializeField] NpcSO owner;
	[SerializeField] NpcDialogueSO ownerDialogues;
	[SerializeField] List<ShopItem> items;

	public List<ShopItem> Items { get { return items; } }
	public NpcSO Owner { get { return owner; } }
	public NpcDialogueSO OwnerDialogues { get { return ownerDialogues; } }
}

[System.Serializable]
public class ShopItem
{
	public ItemSO Item;
	public int Quantity;
	public int Cost;
}