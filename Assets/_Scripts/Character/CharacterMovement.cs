using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
	public Action<Vector2> OnMoveHero;
	public Action<Vector2> OnChangeHeroDirection;
}
