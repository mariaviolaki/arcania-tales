using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static GameInput;

public class InputHandler : MonoBehaviour, IGameplayActions, IUIActions
{
	GameInput gameInput;

	public Action<Vector2> OnMoveInput;
	public Action<RaycastHit2D[]> OnInteractInput;

	void Awake()
	{
		if (gameInput != null) return;

		gameInput = new GameInput();
		gameInput.Gameplay.SetCallbacks(this);
		gameInput.UI.SetCallbacks(this);

		SetGameplay();
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

	void IGameplayActions.OnMove(InputAction.CallbackContext context)
	{
		OnMoveInput?.Invoke(context.ReadValue<Vector2>());
	}

	void IGameplayActions.OnInteract(InputAction.CallbackContext context)
	{
		if (context.phase != InputActionPhase.Performed) return;

		// Only process non-UI clicks
		Vector2 position = context.ReadValue<Vector2>();
		if (IsUIPosition(position)) return;

		// Get all game objects on the click position (works only for objects with colliders)
		RaycastHit2D[] hitTargets = GetRaycastWorldTargets(position);
		OnInteractInput?.Invoke(hitTargets);
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

	void IUIActions.OnNavigate(InputAction.CallbackContext context)
	{
		Debug.Log("OnNavigate");
	}

	void IUIActions.OnSubmit(InputAction.CallbackContext context)
	{
		Debug.Log("OnSubmit");
	}

	void IUIActions.OnCancel(InputAction.CallbackContext context)
	{
		Debug.Log("OnCancel");
	}

	void IUIActions.OnPoint(InputAction.CallbackContext context)
	{
		Debug.Log("OnPoint");
	}

	void IUIActions.OnClick(InputAction.CallbackContext context)
	{
		Debug.Log("OnClick");
	}

	void IUIActions.OnScrollWheel(InputAction.CallbackContext context)
	{
		Debug.Log("OnScrollWheel");
	}

	void IUIActions.OnMiddleClick(InputAction.CallbackContext context)
	{
		Debug.Log("OnMiddleClick");
	}

	void IUIActions.OnRightClick(InputAction.CallbackContext context)
	{
		Debug.Log("OnRightClick");
	}

	void IUIActions.OnTrackedDevicePosition(InputAction.CallbackContext context)
	{
		Debug.Log("OnTrackedDevicePosition");
	}

	void IUIActions.OnTrackedDeviceOrientation(InputAction.CallbackContext context)
	{
		Debug.Log("OnTrackedDeviceOrientation");
	}
}
