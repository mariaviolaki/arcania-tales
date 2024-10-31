using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseDialogueUI : CanvasUI
{
	[SerializeField] protected TMP_Text dialogueField;
	[SerializeField] GameObject dialogueArea;

	bool hasInteracted;

	abstract protected void ContinueDialogueUI();

	override protected void Awake()
	{
		base.Awake();

		hasInteracted = false;
		InitDialogueArea();
	}

	void InitDialogueArea()
	{
		// Click anywhere on the game screen to progress the dialogue
		EventTrigger eventTrigger = dialogueArea.GetComponent<EventTrigger>();
		EventTrigger.Entry clickEvent = new EventTrigger.Entry();
		clickEvent.eventID = EventTriggerType.PointerClick;
		clickEvent.callback.AddListener((data) => ProcessPlayerInteraction((PointerEventData)data));
		eventTrigger.triggers.Add(clickEvent);
	}

	void ProcessPlayerInteraction(PointerEventData eventData)
	{
		// Don't progress the dialogue if the player clicks too fast
		if (hasInteracted) return;

		hasInteracted = true;
		ContinueDialogueUI();
		hasInteracted = false;
	}
}
