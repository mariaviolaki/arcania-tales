using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonUI : MonoBehaviour, IPointerClickHandler
{
	public Action OnPointerClickUI;

	public void OnPointerClick(PointerEventData eventData)
	{
		OnPointerClickUI?.Invoke();
	}
}
