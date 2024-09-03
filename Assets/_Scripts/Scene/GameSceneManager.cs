using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
	[SerializeField] GameplaySettingsSO gameplaySettings;

	GameEnums.Scene currentScene;
	AsyncOperation asyncUnload;
	AsyncOperation asyncLoad;

	public Action OnBeginChangeScene;
	public Action<Vector2> OnEndChangeScene;

	public GameEnums.Scene CurrentScene { get { return currentScene; } }

	void Awake()
    {
		InitSceneData();
	}

	public void ChangeScene(GameEnums.Scene newGameScene, Vector2 entryPoint)
	{
		string oldSceneName = Enum.GetName(typeof(GameEnums.Scene), currentScene);
		StartCoroutine(TransitionToScene(newGameScene, oldSceneName, entryPoint));
	}

	void InitSceneData()
	{
		string sceneName = SceneManager.GetActiveScene().name;
		Enum.TryParse(sceneName, out currentScene);
	}

	IEnumerator TransitionToScene(GameEnums.Scene newGameScene, string oldSceneName, Vector2 entryPoint)
	{
		OnBeginChangeScene?.Invoke();
		yield return new WaitForSeconds(gameplaySettings.SceneTransitionDelay);

		// The new scene has to be loaded before unloading the previous one (there must always be at least 1)
		string newSceneName = Enum.GetName(typeof(GameEnums.Scene), newGameScene);
		asyncLoad = SceneManager.LoadSceneAsync(newSceneName, LoadSceneMode.Additive);
		while (asyncLoad != null)
		{
			if (asyncLoad.isDone) asyncLoad = null;
			yield return null;
		}

		// Complete the transition to the new scene before notifying any observers
		asyncUnload = SceneManager.UnloadSceneAsync(oldSceneName);
		while (asyncUnload != null)
		{
			if (asyncUnload.isDone) asyncUnload = null;
			yield return null;
		}

		currentScene = newGameScene;
		OnEndChangeScene?.Invoke(entryPoint);
	}
}
