using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
	InputHandler inputHandler;
	
	void Awake()
	{
		inputHandler = GetComponent<InputHandler>();
		inputHandler.OnInteractInput += ProcessInteractions;
	}

	void ProcessInteractions(RaycastHit2D[] targets)
	{
		foreach (RaycastHit2D target in targets)
		{
			IInteractable interactable = target.transform.GetComponent<IInteractable>();
			interactable?.Interact();
		}
	}
}
