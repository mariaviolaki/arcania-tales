using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
	[SerializeField] GameplaySettingsSO gameplaySettings;

	GameEnums.Scene lastScene;
	GameEnums.Scene currentScene;
	AsyncOperation asyncUnload;
	AsyncOperation asyncLoad;

	bool isChangingScene;

	public Action OnLoadGameScenes;
	public Action OnUnloadGameScenes;
	public Action OnBeginChangeScene;
	public Action<Vector2> OnEndChangeScene;

	public GameEnums.Scene CurrentScene { get { return currentScene; } }
	public GameEnums.Scene LastScene { get { return lastScene; } }

	void Awake()
    {
		isChangingScene = false;
		InitSceneData();
	}

	public void ChangeScene(GameEnums.Scene newGameScene, Vector2 entryPoint)
	{
		if (newGameScene == GameEnums.Scene.None || isChangingScene) return;

		isChangingScene = true;
		string oldSceneName = Enum.GetName(typeof(GameEnums.Scene), currentScene);
		StartCoroutine(TransitionToScene(newGameScene, oldSceneName, entryPoint));
	}

	public IEnumerator LoadGameScenes()
	{
		foreach (GameEnums.Scene scene in Enum.GetValues(typeof(GameEnums.Scene)))
		{
			if (scene == GameEnums.Scene.None || scene == currentScene) continue;

			string sceneName = Enum.GetName(typeof(GameEnums.Scene), scene);
			asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
			while (asyncLoad != null)
			{
				if (asyncLoad.isDone) asyncLoad = null;
				yield return null;
			}
		}

		OnLoadGameScenes?.Invoke();
	}

	public IEnumerator UnloadGameScenes()
	{
		foreach (GameEnums.Scene scene in Enum.GetValues(typeof(GameEnums.Scene)))
		{
			if (scene == GameEnums.Scene.None || scene == currentScene) continue;

			string sceneName = Enum.GetName(typeof(GameEnums.Scene), scene);
			asyncUnload = SceneManager.UnloadSceneAsync(sceneName);
			while (asyncUnload != null)
			{
				if (asyncUnload.isDone) asyncUnload = null;
				yield return null;
			}
		}

		OnUnloadGameScenes?.Invoke();
	}

	void InitSceneData()
	{
		lastScene = GameEnums.Scene.None;

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

		lastScene = currentScene;
		currentScene = newGameScene;
		isChangingScene = false;
		OnEndChangeScene?.Invoke(entryPoint);
	}
}
