using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueSequence
{
	[SerializeField] List<DialoguePart> dialogueParts;

	public List<DialoguePart> DialogueParts { get { return dialogueParts; } }
}

[System.Serializable]
public class DialoguePart
{
	[SerializeField][TextArea(3, 3)] string text;
	[SerializeField] GameEnums.NpcExpression expression;

	public string Text { get { return text; } }
	public GameEnums.NpcExpression Expression { get { return expression; } }
}
