using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class ItemSO : ScriptableObject
{
	[SerializeField] string itemName;
	[SerializeField] string description;
	[SerializeField] Sprite image;
	[SerializeField] int respawnHours;

    public string Name { get { return itemName; } }
	public string Description { get { return description; } }
	public Sprite Image { get { return image; } }
	public int RespawnHours { get { return respawnHours; } }
}
