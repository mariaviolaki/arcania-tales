using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueUI : CanvasUI
{
	[SerializeField] Image npcImage;
	[SerializeField] TMP_Text npcNameField;
	[SerializeField] TMP_Text dialogueField;
	[SerializeField] GameObject dialogueArea;

	public Action OnOpenDialogueUI;
	public Action OnContinueDialogue;
	public Action OnCloseDialogueUI;

	override protected void Awake()
	{
		base.Awake();
		InitDialogueArea();
	}

	public void StartNpcDialogue(string npcName, string npcDialogue, Sprite npcSprite)
	{
		ContinueNpcDialogue(npcName, npcDialogue, npcSprite);

		OnOpenDialogueUI?.Invoke();
	}

	public void ContinueNpcDialogue(string npcName, string npcDialogue, Sprite npcSprite)
	{
		npcNameField.text = npcName;
		dialogueField.text = npcDialogue;
		npcImage.sprite = npcSprite;
	}

	public void EndNpcDialogue()
	{
		OnCloseDialogueUI?.Invoke();
	}

	void InitDialogueArea()
	{
		// Click anywhere in the game screen to progress the dialogue
		EventTrigger eventTrigger = dialogueArea.GetComponent<EventTrigger>();
		EventTrigger.Entry clickEvent = new EventTrigger.Entry();
		clickEvent.eventID = EventTriggerType.PointerClick;
		clickEvent.callback.AddListener((data) => ContinueDialogueUI((PointerEventData)data));
		eventTrigger.triggers.Add(clickEvent);
	}

	void ContinueDialogueUI(PointerEventData eventData)
	{
		OnContinueDialogue?.Invoke();
	}
}
