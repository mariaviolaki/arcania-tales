using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
	[SerializeField] GameSettingsSO gameSettings;
	[SerializeField] InputHandlerSO inputHandler;
	
	void Awake()
	{
		inputHandler.OnInteractInput += ProcessInteractions;
	}

	void ProcessInteractions(RaycastHit2D[] targets)
	{
		foreach (RaycastHit2D target in targets)
		{
			// The player cannot interact with this type of object
			IInteractable interactable = target.transform.GetComponent<IInteractable>();
			if (interactable == null) continue;

			// This object is too far away
			float distanceFromObject = Vector2.Distance(transform.position, target.transform.root.position);
			if (distanceFromObject > gameSettings.InteractDistance) continue;

			interactable.Interact(transform);
		}
	}
}
