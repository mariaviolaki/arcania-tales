using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NpcDialogue", menuName = "Scriptable Objects/Npc Dialogue")]
public class NpcDialogueSO : ScriptableObject
{
	[SerializeField] List<DialogueSequence> greetings;
	[SerializeField] List<DialogueSequence> repeatGreetings;

	public DialogueSequence GetGreeting()
	{
		int dialogueIndex = Random.Range(0, greetings.Count);
		return greetings[dialogueIndex];
	}

	public DialogueSequence GetRepeatGreeting()
	{
		int dialogueIndex = Random.Range(0, repeatGreetings.Count);
		return repeatGreetings[dialogueIndex];
	}
}
