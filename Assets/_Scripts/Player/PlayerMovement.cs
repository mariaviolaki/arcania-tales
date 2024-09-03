using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] InputHandlerSO inputHandler;
	[SerializeField] GameplaySettingsSO gameplaySettings;
	[SerializeField] GameSceneManager sceneManager;
	
	Rigidbody2D cRigidbody;
	Vector2 moveInput;
	bool isMovementEnabled;

	public Action<Vector2> OnMovePlayer;

	void Awake()
	{
		cRigidbody = GetComponent<Rigidbody2D>();
		inputHandler.OnGameMoveInput += SaveMoveInput;
		sceneManager.OnBeginChangeScene += PausePlayerMovement;
		sceneManager.OnEndChangeScene += MoveToSceneEntry;

		isMovementEnabled = true;
	}

	void Update()
    {
		Walk();
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		int collisionLayer = collision.gameObject.layer;
		bool isSceneEntry = collisionLayer == LayerMask.NameToLayer(GameConstants.CollisionLayers.SceneEntry);
		if (isSceneEntry)
		{
			ChangeScene(collision.gameObject);
		}
	}

	void SaveMoveInput(Vector2 moveInput)
	{
		if (!isMovementEnabled) return;

		this.moveInput = moveInput;
		OnMovePlayer?.Invoke(moveInput);
	}

	void Walk()
	{
		Vector2 playerVelocity = moveInput * gameplaySettings.PlayerWalkSpeed;
		cRigidbody.velocity = playerVelocity;
	}

	void ChangeScene(GameObject sceneEntryObject)
	{
		SceneEntry sceneEntry = sceneEntryObject.GetComponent<SceneEntry>();
		if (sceneEntry != null && sceneManager != null &&  sceneEntry.NextScene != GameEnums.Scene.None)
		{
			sceneManager.ChangeScene(sceneEntry.NextScene, sceneEntry.EntryPoint);
		}			
	}

	void PausePlayerMovement()
	{
		SaveMoveInput(Vector2.zero);
		isMovementEnabled = false;
	}

	void MoveToSceneEntry(Vector2 entryPoint)
	{
		transform.position = entryPoint;
		isMovementEnabled = true;
	}
}
