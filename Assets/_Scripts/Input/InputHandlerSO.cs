using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputHandler", menuName = "Scriptable Objects/Input Handler")]
public class InputHandlerSO : ScriptableObject, GameInput.ISharedActions, GameInput.IGameplayActions, GameInput.IUIActions
{
	GameInput gameInput;
	Vector2 lastSelectPosition;

	public Action<bool> OnSelectInput;
	public Action<Vector2> OnGameMoveInput;
	public Action<RaycastHit2D[]> OnGameSelectInput;
	public Action<Vector2> OnUIMoveInput;

	public Vector2 LastSelectPosition { get { return lastSelectPosition; } }

	void OnEnable()
	{
		if (gameInput == null)
		{
			gameInput = new GameInput();
		}

		gameInput.Shared.SetCallbacks(this);
		gameInput.Gameplay.SetCallbacks(this);
		gameInput.UI.SetCallbacks(this);

		gameInput.Shared.Enable();
		gameInput.Gameplay.Enable();
		gameInput.UI.Disable();
	}

	void OnDisable()
	{
		gameInput.Shared.RemoveCallbacks(this);
		gameInput.Gameplay.RemoveCallbacks(this);
		gameInput.UI.RemoveCallbacks(this);
	}

	public void SetGameplayEnabled(bool isEnabled)
	{
		if (isEnabled)
		{
			gameInput.Gameplay.Enable();
		}
		else
		{
			gameInput.Gameplay.Disable();
		}
	}

	public void SetUIEnabled(bool isEnabled)
	{
		if (isEnabled)
		{
			gameInput.UI.Enable();
		}
		else
		{
			gameInput.UI.Disable();
		}
	}

	void GameInput.ISharedActions.OnSelect(InputAction.CallbackContext context)
	{
		if (context.phase != InputActionPhase.Performed) return;

		Vector2 position = context.ReadValue<Vector2>();
		lastSelectPosition = position;

		bool isUIPosition = IsUIPosition(position);

		// Allow the UI Manager to enable custom UI events
		OnSelectInput?.Invoke(isUIPosition);

		// Send position information for in-world objects
		if (!isUIPosition)
		{
			// Get all game objects on the click position (works only for objects with colliders)
			RaycastHit2D[] hitTargets = GetRaycastWorldTargets(position);
			OnGameSelectInput?.Invoke(hitTargets);
		}
	}

	void GameInput.IGameplayActions.OnMove(InputAction.CallbackContext context)
	{
		OnGameMoveInput?.Invoke(context.ReadValue<Vector2>());
	}

	void GameInput.IGameplayActions.OnPause(InputAction.CallbackContext context)
	{
		if (context.phase != InputActionPhase.Performed) return;

		SetGameplayEnabled(false);
		SetUIEnabled(true);
		Debug.Log("Game Paused");
	}

	void GameInput.IUIActions.OnResume(InputAction.CallbackContext context)
	{
		if (context.phase != InputActionPhase.Performed) return;

		SetGameplayEnabled(true);
		SetUIEnabled(false);
		Debug.Log("Game Resumed");
	}

	void GameInput.IUIActions.OnNavigate(InputAction.CallbackContext context)
	{
		if (context.phase != InputActionPhase.Performed) return;

		Vector2 position = context.ReadValue<Vector2>();
		OnUIMoveInput?.Invoke(position);
	}

	void GameInput.IUIActions.OnTrackedDeviceOrientation(InputAction.CallbackContext context)
	{
		
	}

	void GameInput.IUIActions.OnTrackedDevicePosition(InputAction.CallbackContext context)
	{
		
	}

	void GameInput.IUIActions.OnRightClick(InputAction.CallbackContext context)
	{
		
	}

	void GameInput.IUIActions.OnMiddleClick(InputAction.CallbackContext context)
	{
		
	}

	void GameInput.IUIActions.OnScrollWheel(InputAction.CallbackContext context)
	{
		
	}

	void GameInput.IUIActions.OnClick(InputAction.CallbackContext context)
	{
		
	}

	void GameInput.IUIActions.OnPoint(InputAction.CallbackContext context)
	{
		
	}

	void GameInput.IUIActions.OnCancel(InputAction.CallbackContext context)
	{
		
	}

	void GameInput.IUIActions.OnSubmit(InputAction.CallbackContext context)
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
