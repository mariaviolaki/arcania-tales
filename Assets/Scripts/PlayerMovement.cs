using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] float walkSpeed = 600f;

	Rigidbody2D cRigidbody;
	Vector2 moveInput;

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
	}

	void Walk()
	{
		Vector2 playerVelocity = moveInput * Time.deltaTime * walkSpeed;
		cRigidbody.velocity = playerVelocity;
	}
}
