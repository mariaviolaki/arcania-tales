using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable
{
	ItemSO item;
	Vector2 position;
	SceneCollectable interactable;
	GameTime interactionTime;

	public ItemSO Item { get { return item; } }
	public Vector2 Position { get { return position; } }
	public GameTime InteractionTime { get { return interactionTime; } set { interactionTime = value; } }
	public SceneCollectable Interactable { get { return interactable; } set { interactable = value; } }

	public Collectable(ItemSO item, Vector2 position, SceneCollectable interactable = null, GameTime interactionTime = null)
	{
		this.item = item;
		this.position = position;
		this.interactable = interactable;
		this.interactionTime = interactionTime;
	}
}
