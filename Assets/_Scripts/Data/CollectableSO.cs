using UnityEngine;

[CreateAssetMenu(fileName = "Collectable", menuName = "Scriptable Objects/Collectable")]
public class CollectableSO : ScriptableObject
{
	[SerializeField] string itemName;
	[SerializeField] string description;
	[SerializeField] Sprite image;

    public string Name { get { return itemName; } }
	public string Description { get { return description; } }
	public Sprite Image { get { return image; } }
}
