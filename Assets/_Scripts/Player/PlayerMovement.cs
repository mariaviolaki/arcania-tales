using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] InputHandlerSO inputHandler;
	[SerializeField] GameplaySettingsSO gameplaySettings;
	
	Rigidbody2D cRigidbody;
	Vector2 moveInput;

	public Action<Vector2> OnMovePlayer;

	void Awake()
	{
		cRigidbody = GetComponent<Rigidbody2D>();
		inputHandler.OnGameMoveInput += SaveMoveInput;
	}

	void Update()
    { 
		Walk();
	}

	void SaveMoveInput(Vector2 moveInput)
	{
		this.moveInput = moveInput;
		OnMovePlayer?.Invoke(moveInput);
	}

	void Walk()
	{
		Vector2 playerVelocity = moveInput * gameplaySettings.PlayerWalkSpeed;
		cRigidbody.velocity = playerVelocity;
	}
}
