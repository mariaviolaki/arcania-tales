using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] float walkSpeed = 5f;

	Rigidbody2D cRigidbody;
	Vector2 moveInput;

	public Action<Vector2> OnMoveInput;

	void Awake()
	{
		cRigidbody = GetComponent<Rigidbody2D>();
	}

	void Update()
    { 
		Walk();
	}

	void OnMove(InputValue value)
	{
		moveInput = value.Get<Vector2>();
		OnMoveInput?.Invoke(moveInput);
	}

	void Walk()
	{
		Vector2 playerVelocity = moveInput * walkSpeed;
		cRigidbody.velocity = playerVelocity;
	}
}
