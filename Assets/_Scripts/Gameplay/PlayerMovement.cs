using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] InputHandlerSO inputHandler;
	[SerializeField] GameplaySettingsSO gameplaySettings;
	
	Rigidbody2D cRigidbody;
	Vector2 moveInput;

	void Awake()
	{
		cRigidbody = GetComponent<Rigidbody2D>();
		inputHandler.OnGameMoveInput += SetPlayerVelocity;
	}

	void Update()
    { 
		Walk();
	}

	void SetPlayerVelocity(Vector2 moveInput)
	{
		this.moveInput = moveInput;
	}

	void Walk()
	{
		Vector2 playerVelocity = moveInput * gameplaySettings.WalkSpeed;
		cRigidbody.velocity = playerVelocity;
	}
}
