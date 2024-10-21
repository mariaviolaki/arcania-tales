using UnityEngine;

public class PlayerMovement : CharacterMovement
{
	[SerializeField] InputHandlerSO inputHandler;
	[SerializeField] GameSceneManager sceneManager;
	[SerializeField] GameplaySettingsSO gameplaySettings;

	Rigidbody2D cRigidbody;
	Vector2 moveInput;
	bool isMovementEnabled;

	void Awake()
	{
		inputHandler.OnGameMoveInput += SaveMoveInput;
		sceneManager.OnBeginChangeScene += PausePlayerMovement;
		sceneManager.OnEndChangeScene += MoveToSceneEntry;
		cRigidbody = GetComponent<Rigidbody2D>();

		isMovementEnabled = true;
	}

	void Update()
	{
		Walk();
	}

	void OnDestroy()
	{
		inputHandler.OnGameMoveInput -= SaveMoveInput;
		sceneManager.OnBeginChangeScene -= PausePlayerMovement;
		sceneManager.OnEndChangeScene -= MoveToSceneEntry;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		int collisionLayer = collision.gameObject.layer;
		bool isSceneEntry = collisionLayer == LayerMask.NameToLayer(GameConstants.CollisionLayers.SceneEntry);
		if (isSceneEntry && collision.gameObject != null)
		{
			SceneEntry sceneEntry = collision.gameObject.GetComponent<SceneEntry>();
			ChangeScene(sceneEntry);
		}
	}

	public void ChangeScene(SceneEntry sceneEntry)
	{
		if (sceneEntry == null || sceneManager == null || sceneEntry.NextScene == GameEnums.Scene.None) return;

		Vector2 entryPos = sceneEntry.GetEntryPoint(sceneManager.CurrentScene);
		sceneManager.ChangeScene(sceneEntry.NextScene, entryPos);
	}

	void Walk()
	{
		Vector2 playerVelocity = moveInput * gameplaySettings.PlayerWalkSpeed;
		cRigidbody.velocity = playerVelocity;
	}

	void SaveMoveInput(Vector2 moveInput)
	{
		if (!isMovementEnabled) return;

		this.moveInput = moveInput;
		OnMoveCharacter?.Invoke(moveInput);
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
