using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawner : MonoBehaviour
{
	[SerializeField] CollectableLocationsSO collectableLocations;
	[SerializeField] GameSceneManager sceneManager;

	[Header("Collectable Initialization")]
	[SerializeField] SceneCollectable collectablePrefab;
	[SerializeField] InventoryManager inventoryManager;
	[SerializeField] DateManager dateManager;

	Dictionary<GameEnums.Scene, List<Collectable>> sceneCollectables;

	void Start()
	{
		sceneManager.OnEndChangeScene += (Vector2 sceneEntry) => UpdateSceneCollectables();
		dateManager.OnHourPassed += RespawnCollectables;

		InitCollectables();
	}

	void OnDestroy()
	{
		sceneManager.OnEndChangeScene -= (Vector2 sceneEntry) => UpdateSceneCollectables();
		dateManager.OnHourPassed -= RespawnCollectables;
	}

	void InitCollectables()
	{
		sceneCollectables = new Dictionary<GameEnums.Scene, List<Collectable>>();

		// Save the collectables for each scene as a different dictionary entry
		foreach (var collectableScene in collectableLocations.CollectableScenes)
		{
			GameEnums.Scene scene = collectableScene.Key;
			List<CollectableItemPositions> groupedItemPositions = collectableScene.Value;

			sceneCollectables[scene] = new List<Collectable>();

			// Iterate over all the different item types in the scene
			foreach (CollectableItemPositions itemPositions in groupedItemPositions)
			{
				// Get the position of each item belonging in this type
				foreach (Vector2 position in itemPositions.Positions)
				{
					SceneCollectable interactable = null;

					// Also create the collectable in the world if it spawns in the starting scene
					if (scene == sceneManager.CurrentScene)
					{
						interactable = CreateCollectable(itemPositions.Item, position);
					}

					sceneCollectables[scene].Add(new Collectable(itemPositions.Item, position, interactable));
				}
			}
		}
	}

	void UpdateSceneCollectables()
	{
		DestroySceneCollectables();
		RecreateCollectables();
	}

	// Every few in-game minutes, check if it's time for the scene collectables to respawn
	void RespawnCollectables()
	{
		// Some scenes don't have collectables
		if (!GameUtils.IsOutdoorScene(sceneManager.CurrentScene)) return;

		foreach (Collectable collectable in sceneCollectables[sceneManager.CurrentScene])
		{
			if (collectable.Interactable == null || collectable.Interactable.isActiveAndEnabled) continue;

			GameTime gameTime = dateManager.GetTotalTime();
			GameTime respawnTime = collectable.Item.RespawnTime;
			GameTime interactionTime = collectable.InteractionTime;

			if (interactionTime != null && (interactionTime + respawnTime <= gameTime))
			{
				collectable.Interactable.gameObject.SetActive(true);
				collectable.InteractionTime = null;
			}
		}
	}

	// Update the interaction time for a certain item after a successful interaction from the player
	void SaveCollectableInteraction(SceneCollectable interactable)
	{
		foreach (Collectable collectable in sceneCollectables[sceneManager.CurrentScene])
		{
			if (collectable.Interactable == interactable)
			{
				collectable.InteractionTime = dateManager.GetTotalTime();
				break;
			}
		}
	}

	void DestroySceneCollectables()
	{
		// Some scenes don't have collectables - or this is the first scene change
		if (!GameUtils.IsOutdoorScene(sceneManager.LastScene)) return;

		foreach (Collectable collectable in sceneCollectables[sceneManager.LastScene])
		{
			collectable.Interactable.OnCollectableInteract -= () => SaveCollectableInteraction(collectable.Interactable);
			Destroy(collectable.Interactable.gameObject);
			collectable.Interactable = null;
		}
	}

	// Instantiate new items when changing scenes and save their new scene references
	void RecreateCollectables()
	{
		// Some scenes don't have collectables
		if (!GameUtils.IsOutdoorScene(sceneManager.CurrentScene)) return;

		foreach (Collectable collectable in sceneCollectables[sceneManager.CurrentScene])
		{
			SceneCollectable interactable = CreateCollectable(collectable.Item, collectable.Position, collectable.InteractionTime);
			collectable.Interactable = interactable;
		}
	}

	SceneCollectable CreateCollectable(ItemSO item, Vector2 position, GameTime interactionTime = null)
	{
		SceneCollectable sceneCollectable = Instantiate(collectablePrefab);
		sceneCollectable.Init(inventoryManager, item);
		sceneCollectable.transform.parent = transform;
		sceneCollectable.transform.position = position;

		// Hide the item from the new scene if the player has recently interacted with it
		bool isOnCooldown = interactionTime != null && (interactionTime + item.RespawnTime > dateManager.GetTotalTime());
		if (isOnCooldown)
		{
			sceneCollectable.gameObject.SetActive(false);
		}

		// Create a listener to be notified by the scene item when the player successfully interacts with it
		sceneCollectable.OnCollectableInteract += () => SaveCollectableInteraction(sceneCollectable);

		return sceneCollectable;
	}
}
