using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputHandler", menuName = "Scriptable Objects/Input Handler")]
public class InputHandlerSO : ScriptableObject, GameInput.IGameplayActions, GameInput.IUIActions
{
	GameInput gameInput;

	public Action<Vector2> OnMoveInput;
	public Action<RaycastHit2D[]> OnInteractInput;

	void OnEnable()
	{
		if (gameInput == null)
		{
			gameInput = new GameInput();
		}
		
		gameInput.Gameplay.SetCallbacks(this);
		gameInput.UI.SetCallbacks(this);

		SetGameplay();
	}

	void OnDisable()
	{
		gameInput.Gameplay.RemoveCallbacks(this);
		gameInput.UI.RemoveCallbacks(this);
	}

	public void SetGameplay()
	{
		gameInput.Gameplay.Enable();
		gameInput.UI.Disable();
	}

	public void SetUI()
	{
		gameInput.Gameplay.Disable();
		gameInput.UI.Enable();
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		OnMoveInput?.Invoke(context.ReadValue<Vector2>());
	}

	public void OnInteract(InputAction.CallbackContext context)
	{
		if (context.phase != InputActionPhase.Performed) return;

		// Only process non-UI clicks
		Vector2 position = context.ReadValue<Vector2>();
		if (IsUIPosition(position)) return;

		// Get all game objects on the click position (works only for objects with colliders)
		RaycastHit2D[] hitTargets = GetRaycastWorldTargets(position);
		OnInteractInput?.Invoke(hitTargets);
	}

	public void OnNavigate(InputAction.CallbackContext context)
	{
		
	}

	public void OnSubmit(InputAction.CallbackContext context)
	{
		
	}

	public void OnCancel(InputAction.CallbackContext context)
	{
		
	}

	public void OnPoint(InputAction.CallbackContext context)
	{
		
	}

	public void OnClick(InputAction.CallbackContext context)
	{
		
	}

	public void OnScrollWheel(InputAction.CallbackContext context)
	{
		
	}

	public void OnMiddleClick(InputAction.CallbackContext context)
	{
		
	}

	public void OnRightClick(InputAction.CallbackContext context)
	{
		
	}

	public void OnTrackedDevicePosition(InputAction.CallbackContext context)
	{
		
	}

	public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
	{
		
	}

	RaycastHit2D[] GetRaycastWorldTargets(Vector2 position)
	{
		Ray ray = Camera.main.ScreenPointToRay(position);
		RaycastHit2D[] hitTargets = Physics2D.RaycastAll(ray.origin, ray.direction, Mathf.Infinity);

		return hitTargets;
	}

	bool IsUIPosition(Vector2 position)
	{
		PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
		pointerEventData.position = position;

		List<RaycastResult> uiResults = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointerEventData, uiResults);

		return uiResults.Count > 0;
	}
}
