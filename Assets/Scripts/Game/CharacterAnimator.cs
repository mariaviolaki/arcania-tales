using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
	[System.Serializable]
	private struct Skins
	{
		public Sprite[] sprites;
	}

	[Tooltip("A single customizable part of a character")]
	[SerializeField] BodyPart bodyPart = BodyPart.None;
	[Tooltip("All the custom sliced sprite sheets for this body part")]
	[SerializeField] Skins[] skins;

	SpriteRenderer spriteRenderer;
	Animator animator;
	PlayerMovement playerMovement;
	int skinNum;

	void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
		playerMovement = transform.parent.GetComponent<PlayerMovement>();
		skinNum = GetSkinNumber();
	}

	void Start()
	{
		playerMovement.OnMoveInput += SelectAnimation;
		SetStartAnimation();
	}

	void LateUpdate()
	{
		ApplySkin();
	}

	void SetStartAnimation()
	{
		animator.SetFloat(nameof(LastMoveDirection.LastHorizontal), 0);
		animator.SetFloat(nameof(LastMoveDirection.LastVertical), -1);
	}

	// Change the current sprite set by the animator based on the selected skin
	void ApplySkin()
	{
		if (bodyPart == BodyPart.None || spriteRenderer.sprite == null) return;

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
		animator.SetFloat(nameof(MoveDirection.Horizontal), moveInput.x);
		animator.SetFloat(nameof(MoveDirection.Vertical), moveInput.y);

		if (moveInput.x != 0 || moveInput.y != 0)
		{
			animator.SetFloat(nameof(LastMoveDirection.LastHorizontal), moveInput.x);
			animator.SetFloat(nameof(LastMoveDirection.LastVertical), moveInput.y);
		}
	}
}
