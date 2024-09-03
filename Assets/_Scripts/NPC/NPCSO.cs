using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC", menuName = "Scriptable Objects/NPC")]
public class NPCSO : ScriptableObject
{
	[SerializeField] string characterName;
	[SerializeField] NPCScheduleSO routeSchedule;

	[Header("Animated Body Parts")]
	[SerializeField] Sprite bodySprite;
	[SerializeField] Sprite hairSprite;
	[SerializeField] Sprite topSprite;
	[SerializeField] Sprite bottomSprite;

	public string CharacterName { get { return characterName; } }
	public NPCScheduleSO RouteSchedule { get { return routeSchedule; } }
	public Sprite BodySprite { get { return bodySprite; } }
	public Sprite HairSprite { get { return hairSprite; } }
	public Sprite TopSprite { get { return topSprite; } }
	public Sprite BottomSprite { get { return bottomSprite; } }
}
