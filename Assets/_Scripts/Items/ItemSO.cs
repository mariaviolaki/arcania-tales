using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class ItemSO : ScriptableObject
{
	[SerializeField] string itemName;
	[SerializeField][TextArea(minLines:2, maxLines:2)] string description;
	[SerializeField] int cost;
	[SerializeField] Sprite image;
	[SerializeField] GameTime respawnTime;

    public string Name { get { return itemName; } }
	public string Description { get { return description; } }
	public int Cost { get { return cost; } }
	public Sprite Image { get { return image; } }
	public GameTime RespawnTime { get { return respawnTime; } }
}
