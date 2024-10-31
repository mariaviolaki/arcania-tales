using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HintUI : BaseDialogueUI
{
	public Action OnOpenHintUI;
	public Action OnCloseHintUI;

	public void ShowHint(string hintText)
	{
		dialogueField.text = hintText;

		OnOpenHintUI?.Invoke();
	}

	protected override void ContinueDialogueUI()
	{
		OnCloseHintUI?.Invoke();
	}
}
