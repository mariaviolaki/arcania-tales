using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Npc", menuName = "Scriptable Objects/Npc")]
public class NpcSO : ScriptableObject
{
	[SerializeField] string npcName;
	[SerializeField] GamePosition startPosition;
	[SerializeField] NpcScheduleSO routeSchedule;

	[Header("Sprites")]
	[SerializeField] Sprite happySprite;
	[SerializeField] Sprite neutralSprite;
	[SerializeField] Sprite angrySprite;

	public string NpcName { get { return npcName; } }
	public GamePosition StartPosition { get { return startPosition; } }
	public NpcScheduleSO RouteSchedule { get { return routeSchedule; } }

	public Sprite HappySprite { get { return happySprite; } }
	public Sprite NeutralSprite { get { return neutralSprite; } }
	public Sprite AngrySprite { get { return angrySprite; } }
}
