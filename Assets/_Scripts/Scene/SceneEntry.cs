using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneEntry : MonoBehaviour
{
	[SerializeField] GameEnums.Scene nextScene;
	[SerializeField] Vector2 entryPoint;

	public GameEnums.Scene NextScene { get { return nextScene; } }
	public Vector2 EntryPoint { get { return entryPoint; } }
}
