using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueUI : BaseDialogueUI
{
	[SerializeField] Image npcImage;
	[SerializeField] TMP_Text npcNameField;

	public Action OnOpenDialogueUI;
	public Action OnContinueDialogueUI;
	public Action OnCloseDialogueUI;

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

	override protected void ContinueDialogueUI()
	{
		OnContinueDialogueUI?.Invoke();
	}
}
