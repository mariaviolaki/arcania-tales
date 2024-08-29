using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
	[SerializeField] GameplaySettingsSO gameplaySettings;
	[SerializeField] InputHandlerSO inputHandler;
	
	void Awake()
	{
		inputHandler.OnGameSelectInput += ProcessInteractions;
	}

	void ProcessInteractions(RaycastHit2D[] targets)
	{
		foreach (RaycastHit2D target in targets)
		{
			// The player cannot interact with this type of object
			IInteractable interactable = target.transform.GetComponent<IInteractable>();
			if (interactable == null) continue;

			// This object is too far away
			float distanceFromObject = Vector2.Distance(transform.position, target.transform.position);
			if (distanceFromObject > gameplaySettings.InteractDistance) continue;

			interactable.Interact(transform);
		}
	}
}
