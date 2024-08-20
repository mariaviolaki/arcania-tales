using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Scriptable Objects/Game Settings")]
public class GameSettingsSO : ScriptableObject
{
	[SerializeField] GameplaySettingsSO gameplaySettings;
	[SerializeField] InventorySettingsSO inventorySettings;
	[SerializeField] DateSettingsSO dateSettings;

	public GameplaySettingsSO Gameplay { get { return gameplaySettings; } }
	public InventorySettingsSO Inventory { get { return inventorySettings; } }
	public DateSettingsSO Date { get { return dateSettings; } }
}
