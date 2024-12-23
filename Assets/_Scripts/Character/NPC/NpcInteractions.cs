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
		dialogueUI.OnContinueDialogueUI += ContinueDialogue;
	}

	void OnDestroy()
	{
		dateManager.OnHourPassed -= ClearDailyInteraction;
		dialogueUI.OnContinueDialogueUI -= ContinueDialogue;
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
		Sprite sprite = dialoguePart.GetSprite(npcInfo);

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
		Sprite sprite = dialoguePart.GetSprite(npcInfo);

		dialogueUI.ContinueNpcDialogue(npcInfo.name, dialoguePart.Text, sprite);
	}

	IEnumerator EndDialogue()
	{
		dialogueUI.EndNpcDialogue();
		currentDialogue = null;

		yield return new WaitForSeconds(settings.DialogueEndDelay);

		// Perform a last check in case the player has quickly initiated a new dialogue
		if (currentDialogue == null)
		{
			movement.SetSchedulePaused(false);
		}
	}

	void ClearDailyInteraction()
	{
		if (hasInteractedToday && dateManager.GetTime() == new GameTime(0, 0))
		{
			hasInteractedToday = false;
		}
	}
}
