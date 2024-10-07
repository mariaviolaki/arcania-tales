using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CollectableLocations", menuName = "Scriptable Objects/Collectable Locations")]
public class CollectableLocationsSO : ScriptableObject
{
	[SerializeField] List<SceneCollectablesSO> scenes;

	Dictionary<GameEnums.Scene, List<CollectableItemPositions>> collectableScenes = new Dictionary<GameEnums.Scene, List<CollectableItemPositions>>();

	public Dictionary<GameEnums.Scene, List<CollectableItemPositions>> CollectableScenes { get { return collectableScenes; } }

	void OnEnable()
	{
		foreach (SceneCollectablesSO scene in scenes)
		{
			collectableScenes.Add(scene.Scene, scene.Collectables);
		}
	}
}
