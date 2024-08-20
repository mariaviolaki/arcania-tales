using UnityEngine;

[CreateAssetMenu(fileName = "GameplaySettings", menuName = "Scriptable Objects/Gameplay Settings")]
public class GameplaySettingsSO : ScriptableObject
{
	[SerializeField] float walkSpeed;
	[SerializeField] float interactDistance;
	[SerializeField] float magnetForce;
	[SerializeField] float magnetDistance;

	public float WalkSpeed { get { return walkSpeed; } }
	public float InteractDistance { get { return interactDistance; } }
	public float MagnetForce { get { return magnetForce; } }
	public float MagnetDistance { get { return magnetDistance; } }
}
