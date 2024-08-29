using UnityEngine;

[CreateAssetMenu(fileName = "CollisionLayers", menuName = "Scriptable Objects/Collision Layers")]
public class CollisionLayersSO : ScriptableObject
{
	[SerializeField] string terrain;
	[SerializeField] string player;
	[SerializeField] string npc;
	[SerializeField] string playerBody;
	[SerializeField] string npcBody;
	[SerializeField] string obstacle;
	[SerializeField] string collectable;

	public string Terrain { get { return terrain; } }
	public string Player { get { return player; } }
	public string NPC { get { return npc; } }
	public string PlayerBody { get { return playerBody; } }
	public string NPCBody { get { return npcBody; } }
	public string Obstacle { get { return obstacle; } }
	public string Collectable { get { return collectable; } }
}
