using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NpcMovement))]
public class NpcInteractions : MonoBehaviour, IInteractable
{
	[SerializeField] GameplaySettingsSO settings;
	[SerializeField] NpcSO npcInfo;
	[SerializeField] NpcDialogueSO npcDialogue;
	[SerializeField] DateManager dateManager;
	[SerializeField] DialogueUI dialogueUI;

	bool hasInteractedToday;
	DialogueSequence currentDialogue;
	int dialogueIndex;
	NpcMovement movement;

	void Awake()
	{
		movement = GetComponent<NpcMovement>();
		hasInteractedToday = false;

		dateManager.OnHourPassed += ClearDailyInteraction;
		dialogueUI.OnContinueDialogue += ContinueDialogue;
	}

	public void Interact(Transform player)
	{
		// Pause the current movement schedule and face the player
		Vector2 facingDirection = (player.position - transform.position).normalized;
		movement.SetSchedulePaused(true, facingDirection);

		StartDialogue();
	}

	void StartDialogue()
	{
		dialogueIndex = 0;

		if (hasInteractedToday)
		{
			currentDialogue = npcDialogue.GetRepeatGreeting();
		}
		else
		{
			currentDialogue = npcDialogue.GetGreeting();
		}

		if (currentDialogue == null) return;

		hasInteractedToday = true;
		DialoguePart dialoguePart = currentDialogue.DialogueParts[dialogueIndex];
		Sprite sprite = GetDialogueSprite(dialoguePart);

		dialogueUI.StartNpcDialogue(npcInfo.name, dialoguePart.Text, sprite);
	}

	void ContinueDialogue()
	{
		if (currentDialogue == null) return;

		dialogueIndex++;

		if (dialogueIndex >= currentDialogue.DialogueParts.Count)
		{
			StartCoroutine(EndDialogue());
			return;
		}

		DialoguePart dialoguePart = currentDialogue.DialogueParts[dialogueIndex];
		Sprite sprite = GetDialogueSprite(dialoguePart);

		dialogueUI.ContinueNpcDialogue(npcInfo.name, dialoguePart.Text, sprite);
	}

	IEnumerator EndDialogue()
	{
		dialogueUI.EndNpcDialogue();

		yield return new WaitForSeconds(settings.DialogueEndDelay);
		
		movement.SetSchedulePaused(false);
	}

	void ClearDailyInteraction()
	{
		if (hasInteractedToday && dateManager.GetTime() == new GameTime(0, 0))
		{
			hasInteractedToday = false;
		}
	}

	Sprite GetDialogueSprite(DialoguePart dialoguePart)
	{
		string expressionName = Enum.GetName(typeof(GameEnums.NpcExpression), dialoguePart.Expression);
		Sprite sprite = (Sprite)npcInfo.GetType().GetProperty(expressionName + "Sprite").GetValue(npcInfo);

		return sprite;
	}
}
