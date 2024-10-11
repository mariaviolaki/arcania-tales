using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
	[System.Serializable]
	private struct Skins
	{
		public Sprite[] sprites;
	}

	[Tooltip("A single customizable part of a character")]
	[SerializeField] GameEnums.BodyPart bodyPart;
	[Tooltip("All the custom sliced sprite sheets for this body part")]
	[SerializeField] Skins[] skins;

	SpriteRenderer spriteRenderer;
	Animator animator;
	CharacterMovement movement;
	int skinNum;

	void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
		movement = transform.parent.GetComponent<CharacterMovement>();
	}

	void Start()
	{
		skinNum = GetSkinNumber();

		InitMovementListeners();
		SetStartAnimation();
	}

	void LateUpdate()
	{
		ApplySkin();
	}

	void OnDestroy()
	{
		if (movement != null)
		{
			movement.OnMoveCharacter -= SelectAnimation;
			movement.OnChangeCharacterDirection -= SetIdleAnimation;
		}
	}

	void InitMovementListeners()
	{
		if (movement != null)
		{
			movement.OnMoveCharacter += SelectAnimation;
			movement.OnChangeCharacterDirection += SetIdleAnimation;
		}
	}

	void SetStartAnimation()
	{
		animator.SetFloat(nameof(GameEnums.LastMoveDirection.LastHorizontal), 0);
		animator.SetFloat(nameof(GameEnums.LastMoveDirection.LastVertical), -1);
	}

	// Change the current sprite set by the animator based on the selected skin
	void ApplySkin()
	{
		if (spriteRenderer.sprite == null) return;

		string spriteName = spriteRenderer.sprite.name;
		int skinFrame = int.Parse(spriteName.Split("_")[1]);

		spriteRenderer.sprite = skins[skinNum].sprites[skinFrame];
	}

	// Parse the skin number from the sprite name shown in the inspector
	int GetSkinNumber()
	{
		if (spriteRenderer.sprite == null) return 0;

		string spriteName = spriteRenderer.sprite.name;
		string skinNumString = spriteName.Split("_")[0].Substring(bodyPart.ToString().Length);

		return int.Parse(skinNumString);
	}

	void SelectAnimation(Vector2 moveInput)
	{
		// Used for animations while the character is moving
		animator.SetFloat(nameof(GameEnums.MoveDirection.Horizontal), moveInput.x);
		animator.SetFloat(nameof(GameEnums.MoveDirection.Vertical), moveInput.y);

		// Save the last move direction to use it when the character stops moving
		if (moveInput.x != 0 || moveInput.y != 0)
		{
			SetIdleAnimation(moveInput);
		}
	}

	void SetIdleAnimation(Vector2 direction)
	{
		// The last move direction will be used to find the direction for the idle animations
		animator.SetFloat(nameof(GameEnums.LastMoveDirection.LastHorizontal), direction.x);
		animator.SetFloat(nameof(GameEnums.LastMoveDirection.LastVertical), direction.y);
	}
}
