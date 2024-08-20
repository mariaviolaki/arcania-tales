using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CollectableLocation
{
	public ItemSO item;
	public List<Vector2> locations;
}

[CreateAssetMenu(fileName = "CollectableLocations", menuName = "Scriptable Objects/Collectable Locations")]
public class CollectableLocationsSO : ScriptableObject
{
	[SerializeField] List<CollectableLocation> collectables;

	public List<CollectableLocation> Collectables { get { return collectables; } }
}
