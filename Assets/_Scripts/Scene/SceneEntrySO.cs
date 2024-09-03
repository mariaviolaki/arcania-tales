using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneEntry", menuName = "Scriptable Objects/Scene Entry")]
public class SceneEntrySO : ScriptableObject
{
	[SerializeField] GameEnums.Scene scene;
	[SerializeField] Vector2 position;
	[SerializeField] SceneEntrySO linkedEntry;

	public GameEnums.Scene Scene { get { return scene; } }
	public Vector2 Position { get { return position; } }
	public SceneEntrySO LinkedEntry { get { return linkedEntry; } }
}
