using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GamePosition
{
	public GameEnums.Scene Scene;
	public Vector2 Pos;

	public GamePosition()
	{
		this.Scene = GameEnums.Scene.None;
		this.Pos = Vector2.zero;
	}

	public GamePosition(GameEnums.Scene scene, Vector2 pos)
	{
		this.Scene = scene;
		this.Pos = pos;
	}
}
