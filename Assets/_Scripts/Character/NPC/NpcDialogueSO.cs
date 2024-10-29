using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPC Dialogue", menuName = "Scriptable Objects/NPC Dialogue")]
public class NpcDialogueSO : ScriptableObject
{
	[SerializeField] List<DialogueSequence> greetings;
	[SerializeField] List<DialogueSequence> repeatGreetings;
	[SerializeField] List<DialogueSequence> shopGreetings;
	[SerializeField] List<DialogueSequence> shopPurchaseThanks;
	[SerializeField] List<DialogueSequence> shopSaleThanks;
	[SerializeField] List<DialogueSequence> shopInventoryWarnings;
	[SerializeField] List<DialogueSequence> shopSaleWarnings;

	public DialogueSequence GetGreeting() { return GetDialogueSequence(greetings); }
	public DialogueSequence GetRepeatGreeting() { return GetDialogueSequence(repeatGreetings); }
	public DialogueSequence GetShopGreeting() { return GetDialogueSequence(shopGreetings); }
	public DialogueSequence GetShopPurchaseThanks() { return GetDialogueSequence(shopPurchaseThanks); }
	public DialogueSequence GetShopSaleThanks() { return GetDialogueSequence(shopSaleThanks); }
	public DialogueSequence GetShopInventoryWarning() { return GetDialogueSequence(shopInventoryWarnings); }
	public DialogueSequence GetShopSaleWarning() { return GetDialogueSequence(shopSaleWarnings); }

	DialogueSequence GetDialogueSequence(List<DialogueSequence> dialogueSequenceList)
	{
		if (dialogueSequenceList.Count == 0) return null;

		int dialogueIndex = Random.Range(0, dialogueSequenceList.Count);
		return dialogueSequenceList[dialogueIndex];
	}
}
