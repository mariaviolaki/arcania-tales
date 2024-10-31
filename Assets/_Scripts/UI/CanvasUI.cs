using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CanvasUI : MonoBehaviour
{
	bool isActive;

	public bool IsActive { get { return isActive; } }

	virtual protected void Awake()
	{
		isActive = true;
		SetActive(false);
	}

	virtual protected void Start()
	{
	}

	virtual protected void OnDestroy()
	{
	}

	virtual public void SetActive(bool isActive)
	{
		if (this.isActive != isActive)
		{
			this.isActive = isActive;
			gameObject.SetActive(isActive);
		}
	}
}
