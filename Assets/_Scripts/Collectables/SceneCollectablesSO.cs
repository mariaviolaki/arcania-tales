using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneCollectables", menuName = "Scriptable Objects/Scene Collectables")]
public class SceneCollectablesSO : ScriptableObject
{
	[SerializeField] GameEnums.Scene scene;
	[SerializeField] List<CollectableItemPositions> collectables;

	public GameEnums.Scene Scene { get { return scene; } }
	public List<CollectableItemPositions> Collectables { get { return collectables; } }
}

[System.Serializable]
public class CollectableItemPositions
{
	public ItemSO Item;
	public List<Vector2> Positions;
}
