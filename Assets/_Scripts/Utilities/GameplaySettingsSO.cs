using UnityEngine;

[CreateAssetMenu(fileName = "GameplaySettings", menuName = "Scriptable Objects/Gameplay Settings")]
public class GameplaySettingsSO : ScriptableObject
{
	[SerializeField] float sceneTransitionDelay;
	[SerializeField] float playerWalkSpeed;
	[SerializeField] float npcWalkSpeed;
	[SerializeField] float interactDistance;
	[SerializeField] float magnetForce;
	[SerializeField] float magnetDistance;

	public float SceneTransitionDelay { get { return sceneTransitionDelay; } }
	public float PlayerWalkSpeed { get { return playerWalkSpeed; } }
	public float NpcWalkSpeed { get { return npcWalkSpeed; } }
	public float InteractDistance { get { return interactDistance; } }
	public float MagnetForce { get { return magnetForce; } }
	public float MagnetDistance { get { return magnetDistance; } }
}
