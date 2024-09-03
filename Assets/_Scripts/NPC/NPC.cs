using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
	[SerializeField] NPCSO npcData;
	[SerializeField] SpriteRenderer bodySprite;
	[SerializeField] SpriteRenderer hairSprite;
	[SerializeField] SpriteRenderer topSprite;
	[SerializeField] SpriteRenderer bottomSprite;

	void Awake()
    {
		bodySprite.sprite = npcData.BodySprite;
		hairSprite.sprite = npcData.HairSprite;
		topSprite.sprite = npcData.TopSprite;
		bottomSprite.sprite = npcData.BottomSprite;
	}
}
