using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameManager", menuName = "Scriptable Objects/Game Manager")]
public class GameManagerSO : ScriptableObject
{
	[SerializeField] InputHandlerSO inputHandler;

	bool isPaused;

	public bool IsPaused { get { return isPaused; } }

	public Action OnResume;
	public Action OnPause;

    void OnEnable()
    {
		isPaused = false;
    }

	public void SetGamePaused(bool isPaused)
	{
		this.isPaused = isPaused;
		inputHandler.SetGameplayEnabled(!isPaused);

		if (isPaused)
		{
			OnPause?.Invoke();
		}
		else
		{
			OnResume?.Invoke();
		}
	}
}
