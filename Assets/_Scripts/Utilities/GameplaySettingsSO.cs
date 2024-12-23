using UnityEngine;

[CreateAssetMenu(fileName = "GameplaySettings", menuName = "Scriptable Objects/Gameplay Settings")]
public class GameplaySettingsSO : ScriptableObject
{
	[Tooltip("The max number of scenes to traverse to get from one scene to another")]
	[SerializeField] float maxSceneRoute;
	[SerializeField] float sceneTransitionDelay;
	[SerializeField] float playerWalkSpeed;
	[SerializeField] float characterWalkSpeed;
	[SerializeField] float interactDistance;
	[SerializeField] float shopInteractDistance;
	[SerializeField] float magnetForce;
	[SerializeField] float magnetDistance;
	[SerializeField] float dialogueEndDelay;
	[SerializeField] int startGold;

	public float MaxSceneRoute { get { return maxSceneRoute; } }
	public float SceneTransitionDelay { get { return sceneTransitionDelay; } }
	public float PlayerWalkSpeed { get { return playerWalkSpeed; } }
	public float CharacterWalkSpeed { get { return characterWalkSpeed; } }
	public float InteractDistance { get { return interactDistance; } }
	public float ShopInteractDistance { get { return shopInteractDistance; } }
	public float MagnetForce { get { return magnetForce; } }
	public float MagnetDistance { get { return magnetDistance; } }
	public float DialogueEndDelay { get { return dialogueEndDelay; } }
	public int StartGold { get { return startGold; } }
}
