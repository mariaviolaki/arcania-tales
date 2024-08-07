using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Scriptable Objects/Game Settings")]
public class GameSettingsSO : ScriptableObject
{
	[Header("World Gameplay")]
	[SerializeField] float walkSpeed;
	[SerializeField] float interactDistance;
	[SerializeField] float magnetForce;
	[SerializeField] float magnetDistance;

	[Header("Inventory Capacity")]
	[SerializeField] int toolSlots;
	[SerializeField] int inventorySlots;
	[SerializeField] int maxSlotQuantity;

	public float WalkSpeed { get { return walkSpeed; } }
	public float InteractDistance { get { return interactDistance; } }
	public float MagnetForce { get { return magnetForce; } }
	public float MagnetDistance { get { return magnetDistance; } }

	public int ToolSlots { get { return toolSlots; } }
	public int InventorySlots { get { return inventorySlots; } }
	public int MaxSlotQuantity { get { return maxSlotQuantity; } }
}
