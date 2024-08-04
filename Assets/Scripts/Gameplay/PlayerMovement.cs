using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] float walkSpeed = 5f;

	InputHandler inputHandler;
	Rigidbody2D cRigidbody;
	Vector2 moveInput;

	void Awake()
	{
		inputHandler = GetComponent<InputHandler>();
		cRigidbody = GetComponent<Rigidbody2D>();
	}

	void Start()
	{
		inputHandler.OnMoveInput += SetPlayerVelocity;
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
		Vector2 playerVelocity = moveInput * walkSpeed;
		cRigidbody.velocity = playerVelocity;
	}
}
