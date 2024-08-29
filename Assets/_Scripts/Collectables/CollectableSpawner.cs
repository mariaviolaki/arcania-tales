using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
	[SerializeField] CollectableLocationsSO collectableLocations;
	
	[Header("Collectable Initialization")]
	[SerializeField] Collectable collectablePrefab;
	[SerializeField] Transform container;
	[SerializeField] InventoryManager inventoryManager;
	[SerializeField] DateManager dateManager;

	void Awake()
	{
		SpawnCollectables();
	}

	void SpawnCollectables()
	{
		List<CollectableLocation> collectables = collectableLocations.Collectables;
		foreach (CollectableLocation collectableData in collectables)
		{
			foreach (Vector2 location in collectableData.locations)
			{
				CreateCollectable(collectableData.item, location);
			}		
		}
	}

	Collectable CreateCollectable(ItemSO item, Vector2 location)
	{
		Collectable collectable = Instantiate(collectablePrefab);
		collectable.Init(item, inventoryManager, dateManager);
		collectable.transform.parent = container;
		collectable.transform.position = location;

		return collectable;
	}
}
